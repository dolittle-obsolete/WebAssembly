#if(true)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Dolittle.Assemblies;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;
using Mono.Cecil;

namespace Dolittle.WebAssembly.Packager
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            - Find all files needed to be copied
            - After tree shaking, make sure files that we know shouldn't be shaked away, to actually be there
            - Generate JSON file with an array of the files

            Support Debug 
            - Copy PDBs (Path.ChangeExtension)
            - Copy correct debugger proxies and dependencies

            - Copy default static HTML and JavaScript files
            
            - Filter down to assemblies only used

            - Support profiler

            - Bindings Assemblies
             */

            /*
             
            Optmization: Not copy a file twice
             
             */

            var inputPath = args[0];
            var outputPath = args[1];
            var managedOutputPath = Path.Combine(outputPath, "managed");

            if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);
            if (!Directory.Exists(managedOutputPath)) Directory.CreateDirectory(managedOutputPath);


            var rootAssemblies = new List<Assembly>();

            var rootAssembly = Assembly.LoadFrom(inputPath);
            var paths = new AssemblyPaths(Path.GetDirectoryName(typeof(Program).Assembly.Location), Path.GetDirectoryName(inputPath));
            
            
            rootAssemblies.Add(rootAssembly);

            var assemblyContext = new AssemblyContext(rootAssembly);
            rootAssemblies.AddRange(assemblyContext.GetReferencedAssemblies());

            var rootAssemblyPaths = rootAssemblies.Select(_ => {
                var path = string.Empty;
                if (_.CodeBase == null) path = paths.FindBestMatchFor($"{_.GetName().Name}.dll");
                else
                {
                    var uri = new Uri(_.CodeBase);
                    path = paths.FindBestMatchFor(uri.AbsolutePath);
                }
                return path;
            });

            foreach( var rootAssemblyPath in rootAssemblyPaths )
            {
                Console.WriteLine($"Using Root Assembly : {rootAssemblyPath}");
            }
            

            var filesToCopy = new List<string>();
            var importedFiles = new HashSet<string>();
            var assemblies = new List<string>();

            foreach( var rootAssemblyPath in rootAssemblyPaths )
            {
                
                Import(paths, rootAssemblyPath, importedFiles);
            }

            Import(paths, "WebAssembly.Bindings.dll", importedFiles);
            Import(paths, "netstandard.dll", importedFiles);

            foreach( var importedFile in importedFiles )
            {
                Console.WriteLine($"Imported : {importedFile}");
            }

            filesToCopy.AddRange(importedFiles.Distinct());

            //filesToCopy.Add(Path.Combine(paths.Sdk, "debug", "mono.js"));
            filesToCopy.Add(Path.Combine(paths.Sdk, "debug", "mono.wasm"));
            filesToCopy.Add(Path.Combine(paths.Sdk, "debug", "mono.wasm.map"));
            filesToCopy.Add(Path.Combine(paths.Sdk, "debug", "mono.wast"));

            foreach (var file in filesToCopy.Distinct())
            {
                var filename = Path.GetFileName(file);
                var extension = Path.GetExtension(file).ToLowerInvariant();
                var destination = Path.Combine(
                    outputPath,
                    extension == ".dll" || extension == ".pdb" ? "managed" : string.Empty,
                    filename);

                Console.WriteLine($"Copy '{filename}' (Source: {Path.GetDirectoryName(file)}) to '{destination}'");

                if (extension == ".dll" || extension == ".pdb") assemblies.Add(filename);
                File.Copy(file, destination, true);
            }

            var assembliesFilePath = Path.Combine(outputPath, "assemblies.json");
            var fileList = string.Join(",\n\t\t", assemblies.Select(_ => $"\"{_}\"").ToArray());
            File.WriteAllText(assembliesFilePath, $"[\n\t\t{fileList}\n]");

            var monoConfigPath = Path.Combine(outputPath, "mono-config.js");
            File.WriteAllText(monoConfigPath,
                $"config = {{\n\tvfs_prefix: 'managed',\n\tdeploy_prefix: 'managed',\n\tenable_debugging: 0, \n\tfile_list: [\n\t\t{fileList}\n\t ],\n\tadd_bindings: function() {{ \n\t\tModule.mono_bindings_init ('[WebAssembly.Bindings]WebAssembly.Runtime');\n\t}}\n}}"
            );

            var monoJsSource = Path.Combine(paths.Sdk, "debug", "mono.js");
            var monoJsDestination = Path.Combine(outputPath, "mono.js");
            var monoJs = File.ReadAllText(monoJsSource);
            monoJs = monoJs.Replace("this.mono_wasm_runtime_is_ready=true;debugger","this.mono_wasm_runtime_is_ready=true;");
            monoJs = monoJs.Replace(
"  			this.mono_wasm_runtime_is_ready = true;\n"+
"  			debugger;\n",
"  			this.mono_wasm_runtime_is_ready = true;\n");

            File.WriteAllText(monoJsDestination, monoJs);
        }

        static void Import(AssemblyPaths paths, string file, HashSet<string> files)
        {
            var bestMatchedFile = paths.FindBestMatchFor(file);
            try
            { 
                var filesToImport = GetFilesFor(paths,bestMatchedFile);
                foreach( var fileToAdd in filesToImport )
                {
                    if( !files.Add(fileToAdd)) return;
                }

                var module = Mono.Cecil.ModuleDefinition.ReadModule(bestMatchedFile);
                foreach (var assemblyReference in module.AssemblyReferences)
                {
                    var referenceFile = $"{assemblyReference.Name}.dll";
                    Import(paths, referenceFile, files);
                }
            } 
            catch
            {
                Console.WriteLine($"Skipping references for {file} - {bestMatchedFile}");
            }
        }

        static IEnumerable<string> GetFilesFor(AssemblyPaths paths, string path)
        {
            var files = new List<string>();
            path = paths.FindBestMatchFor(path);
            if (File.Exists(path))
            {
                files.Add(path);

                var pdbFile = Path.ChangeExtension(path, ".pdb");
                if (File.Exists(pdbFile)) files.Add(pdbFile);
            }
            return files;
        }
    }
}
#endif
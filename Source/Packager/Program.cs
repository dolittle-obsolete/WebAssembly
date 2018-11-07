#if(false)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;
using Mono.Cecil;

namespace Packager
{
    class Program
    {
        static void CollectAssemblies()
        {

        }

        static void Main(string[] args)
        {
            /*

            Support Debug 
            - Copy PDBs (Path.ChangeExtension)
            - Copy correct debugger proxies and dependencies

            - Copy default static HTML and JavaScript files
            
            - Filter down to assemblies only used

            - Support profiler

            - Bindings Assemblies
             */

            var path = "/Users/einari/Projects/Dolittle/Interaction/WebAssembly/Samples/Basic/bin/Debug/netstandard2.0/Basic.dll"; //args[0];
            var outputPath = "./publish"; // args[1];
            var 

            //while (!System.Diagnostics.Debugger.IsAttached) System.Threading.Thread.Sleep(20);

            Console.WriteLine("Get referenced assemblies");

            var loader = new AssemblyLoader(path);
            var assemblies = loader.GetReferencedAssemblies();

            foreach (var assembly in assemblies)
            {
                var assemblyName = assembly.GetName();
                var uri = new Uri(assemblyName.CodeBase);
                var sourceFile = 

                var destination = Path.Combine(outputPath, Path.GetFileName(uri.AbsolutePath));
                Console.WriteLine($"Copy '{uri.AbsolutePath}' to '{destination}'");
                //File.Copy(uri.AbsolutePath, destination);
            }

        }

        static void CopyLibrariesToOutput(AssemblyLoadContext context, IEnumerable<Library> libraries, string outputPath)
        {
            var filtered = libraries.Where(_ => _.Path != null && _.Path.Length > 0);

            foreach (var library in filtered)
            {
                var assembly = context.LoadFromAssemblyPath(library.Path);
                Console.WriteLine($"Library : {assembly.CodeBase}");
            }
        }
    }
}
#endif
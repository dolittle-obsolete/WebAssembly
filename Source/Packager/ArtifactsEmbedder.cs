/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Mono.Cecil;

namespace Dolittle.Interaction.WebAssembly.Packager
{
    /// <summary>
    /// Represents a system that can embed the needed Dolittle artifacts as embedded resources 
    /// into the target output assembly
    /// </summary>
    public class ArtifactsEmbedder
    {
        readonly Configuration _configuration;
        readonly Assemblies _assemblies;

        /// <summary>
        /// Initializes a new instance of <see cref="ArtifactsEmbedder"/>
        /// </summary>
        /// <param name="configuration">Current <see cref="Configuration"/> used</param>
        /// <param name="assemblies">All <see cref="Assemblies"/> </param>
        public ArtifactsEmbedder(Configuration configuration, Assemblies assemblies)
        {
            _configuration = configuration;
            _assemblies = assemblies;
        }

        /// <summary>
        /// Perform the embedding
        /// </summary>
        public void Perform()
        {
            var files = new List<string>();
            files.Add(_configuration.BoundedContextFilePath);

            var dolittleDirectory = Path.Combine(Directory.GetCurrentDirectory(), ".dolittle");
            files.AddRange(Directory.GetFiles(dolittleDirectory));

            var outputTarget = Path.Combine(_configuration.ManagedOutputPath, Path.GetFileName(_configuration.EntryAssemblyPath));
            Console.WriteLine($"Adding artifacts to '{outputTarget}'");

            var tempFile = $"{outputTarget}.temp";


            using( var stream = File.OpenRead(outputTarget) )
            {
                using(var assemblyDefinition = AssemblyDefinition.ReadAssembly(stream))
                {
                    foreach (var file in files.Where(File.Exists))
                    {
                        var name = $"{assemblyDefinition.Name.Name}.{Path.GetFileName(file)}";
                        Console.WriteLine($"  Adding resource '{name}'");
                        var embeddedResource = new EmbeddedResource(name, ManifestResourceAttributes.Public, File.OpenRead(file));
                        assemblyDefinition.MainModule.Resources.Add(embeddedResource);
                    }

                    AddAssembliesJson(assemblyDefinition);

                    assemblyDefinition.Write(tempFile);
                }

                stream.Close();
            }

            var attempts = 5;
            do
            {
                if (!IsLocked(outputTarget)) break;
                Console.WriteLine($"File is locked - retrying in 200ms - attempts left #{attempts}");
                Thread.Sleep(200);
                attempts--;
            } while (attempts > 0);

            File.Delete(outputTarget);
            File.Move(tempFile, outputTarget);

        }

        bool IsLocked(string path)
        {
            try
            {
                File.OpenWrite(path).Close();
                return false;
            }

            catch (Exception) { return true; }
        }

        void AddAssembliesJson(AssemblyDefinition assemblyDefinition)
        {
            var fileList = string.Join(",", _assemblies.AllImportedAssemblyPaths.Select(_ => $"\"{Path.GetFileNameWithoutExtension(_)}\"").ToArray());
            var json = $"[{fileList}]";
            var assembliesResource = new EmbeddedResource(
                $"{assemblyDefinition.Name.Name}.assemblies.json",
                ManifestResourceAttributes.Public,
                Encoding.UTF8.GetBytes(json));
            assemblyDefinition.MainModule.Resources.Add(assembliesResource);
        }
    }
}
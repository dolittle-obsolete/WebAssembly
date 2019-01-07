using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace ArtifactsEmbedder
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = new List<string>();
            files.Add(args[1]);

            var dolittleDirectory = Path.Combine(Directory.GetCurrentDirectory(), ".dolittle");
            files.AddRange(Directory.GetFiles(dolittleDirectory));
            files.Add(Path.Combine(args[2],"assemblies.json"));

            var outputTarget = Path.Combine(args[2],"managed",Path.GetFileName(args[0]));
            if( !File.Exists(outputTarget)) outputTarget = args[0];

            Console.WriteLine($"Adding artifacts to '{outputTarget}'");

            var tempFile = $"{outputTarget}.temp";
            using(var assemblyDefinition = AssemblyDefinition.ReadAssembly(outputTarget))
            {
                foreach (var file in files.Where(File.Exists))
                {
                    var name = $"{assemblyDefinition.Name.Name}.{Path.GetFileName(file)}";
                    Console.WriteLine($"  Adding resource '{name}'");
                    var embeddedResource = new EmbeddedResource(name, ManifestResourceAttributes.Public, File.OpenRead(file));
                    assemblyDefinition.MainModule.Resources.Add(embeddedResource);
                }

                assemblyDefinition.Write(tempFile);
            }

            File.Delete(outputTarget);
            File.Move(tempFile, outputTarget);
        }
    }
}
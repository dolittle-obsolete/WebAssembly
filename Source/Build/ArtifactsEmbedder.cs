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
using Dolittle.Build;
using Dolittle.Collections;
using Mono.Cecil;
using Newtonsoft.Json;

namespace Dolittle.Interaction.WebAssembly.Build
{
    /// <summary>
    /// Represents a system that can embed the needed Dolittle artifacts as embedded resources 
    /// into the target output assembly
    /// </summary>
    public class ArtifactsEmbedder
    {
        readonly Configuration _configuration;
        readonly BuildTarget _buildTarget;
        readonly Assemblies _assemblies;
        readonly ITargetAssemblyModifiers _modifiers;
        readonly IBuildMessages _buildMessages;

        /// <summary>
        /// Initializes a new instance of <see cref="ArtifactsEmbedder"/>
        /// </summary>
        /// <param name="configuration">Current <see cref="Configuration"/> used</param>
        /// <param name="buildTarget">Current <see cref="BuildTarget"/> being built</param>
        /// <param name="assemblies">All <see cref="Assemblies"/> </param>
        /// <param name="modifiers"><see cref="ITargetAssemblyModifiers"/> for modifying the target</param>
        /// <param name="buildMessages"><see cref="IBuildMessages"/> to use for outputting build messages</param>
        public ArtifactsEmbedder(
            Configuration configuration,
            BuildTarget buildTarget,
            Assemblies assemblies,
            ITargetAssemblyModifiers modifiers,
            IBuildMessages buildMessages)
        {
            _configuration = configuration;
            _assemblies = assemblies;
            _modifiers = modifiers;
            _buildTarget = buildTarget;
            _buildMessages = buildMessages;
        }

        /// <summary>
        /// Perform the embedding
        /// </summary>
        public void Perform()
        {
            var files = new List<string>
            {
                _configuration.BoundedContextFilePath
            };

            files.AddRange(Directory.GetFiles(_configuration.DolittleFolder));

            OverrideWithLocalFiles(files);

            foreach (var file in files.Where(File.Exists))
            {
                var name = $"{_buildTarget.AssemblyName.Name}.{Path.GetFileName(file)}";
                _buildMessages.Information($"Adding artifact resource '{name}'");
                _modifiers.AddModifier(new EmbedResource(name, File.ReadAllBytes(file)));
            }

            AddAssembliesJson();
            AddLibrariesJson();
        }

        void OverrideWithLocalFiles(List<string> files)
        {
            var overrideDolittleFolder = Path.Combine(Directory.GetCurrentDirectory(), ".dolittle");
            if (Directory.Exists(overrideDolittleFolder))
            {
                var overridden = new Dictionary<string, string>();
                var overrideFiles = Directory.GetFiles(overrideDolittleFolder);
                files.ForEach(file =>
                {
                    overrideFiles.ForEach(overrideFile =>
                    {
                        if (Path.GetFileName(overrideFile) == Path.GetFileName(file))
                        {
                            _buildMessages.Information($"Overriding artifact '{Path.GetFileName(file)}' locally");
                            overridden[file] = overrideFile;
                        }
                    });
                });
                files.RemoveAll(_ => overridden.Keys.Contains(_));
                files.AddRange(overridden.Values);
            }
        }

        void AddAssembliesJson()
        {
            var name = $"{_buildTarget.AssemblyName.Name}.assemblies.json";
            _buildMessages.Information($"Adding assemblies list resource '{name}'");
            var fileList = string.Join(",", _assemblies.AllImportedAssemblyPaths.Select(_ => $"\"{Path.GetFileNameWithoutExtension(_)}\"").ToArray());
            var json = $"[{fileList}]";
            _modifiers.AddModifier(new EmbedResource(name, Encoding.UTF8.GetBytes(json)));
        }

        void AddLibrariesJson()
        {
            var name = $"{_buildTarget.AssemblyName.Name}.libraries.json";
            _buildMessages.Information($"Adding libraries list resource '{name}'");
            var fileList = string.Join(",", _assemblies.AllImportedAssemblyPaths.Select(_ => $"\"{Path.GetFileNameWithoutExtension(_)}\"").ToArray());
            var json = JsonConvert.SerializeObject(_assemblies.Libraries);
            _modifiers.AddModifier(new EmbedResource(name, Encoding.UTF8.GetBytes(json)));
        }
    }
}
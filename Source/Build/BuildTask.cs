/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dolittle.Build;
using Dolittle.Collections;
using Newtonsoft.Json;

namespace Dolittle.Interaction.WebAssembly.Build
{

    /// <summary>
    /// Represents an implementation of <see cref="ICanPerformBuildTask"/>
    /// </summary>
    public class BuildTask : ICanPerformBuildTask
    {
        readonly Configuration _configuration;
        readonly AssemblyPaths _assemblyPaths;
        readonly ArtifactsEmbedder _artifactsEmbedder;
        readonly Assemblies _assemblies;

        /// <summary>
        /// Initializes a new instance of <see cref="BuildTask"/>
        /// </summary>
        /// <param name="configuration"><see cref="Configuration"/> to use</param>
        /// <param name="assemblyPaths"><see cref="AssemblyPaths"/> to use</param>
        /// <param name="assemblies"><see cref="Assemblies"/> for handling what assemblies is part of the deployment</param>
        /// <param name="artifactsEmbedder"><see cref="ArtifactsEmbedder"/> for embedding artifacts</param>
        public BuildTask(
            Configuration configuration,
            AssemblyPaths assemblyPaths,
            Assemblies assemblies,
            ArtifactsEmbedder artifactsEmbedder)
        {
            _configuration = configuration;
            _assemblyPaths = assemblyPaths;
            _artifactsEmbedder = artifactsEmbedder;
            _assemblies = assemblies;
        }

        /// <inheritdoc/>
        public string Message => "Generating and embedding assembly and library maps";

        /// <inheritdoc/>
        public void Perform()
        {
            var configurationPath = _configuration.IsRelease? "release": "debug";

            var managedFiles = new List<string>();
            managedFiles.AddRange(_assemblies.AllImportedAssemblyPaths);
            if (!_configuration.IsRelease) managedFiles.AddRange(_assemblies.AllImportedAssemblyDebugSymbolPaths);

            var librariesFilePath = Path.Combine(_configuration.OutputPath, "libraries.json");
            File.WriteAllText(librariesFilePath, JsonConvert.SerializeObject(_assemblies.Libraries, Formatting.Indented));

            var assembliesFilePath = Path.Combine(_configuration.OutputPath, "assemblies.json");
            var fileList = string.Join(",\n\t\t", managedFiles.Select(_ => $"\"{Path.GetFileName(_)}\"").ToArray());
            File.WriteAllText(assembliesFilePath, $"[\n\t\t{fileList}\n]");

            var monoConfigPath = Path.Combine(_configuration.OutputPath, "mono-config.js");
            var enableDebugging = _configuration.IsRelease?0 : 1;

            File.WriteAllText(monoConfigPath,
                $"config = {{\n\tvfs_prefix: 'managed',\n\tdeploy_prefix: 'managed',\n\tenable_debugging: {enableDebugging}, \n\tfile_list: [\n\t\t{fileList}\n\t ],\n\tadd_bindings: function() {{ \n\t\tModule.mono_bindings_init ('[WebAssembly.Bindings]WebAssembly.Runtime');\n\t}}\n}}"
            );

            var monoJsSource = Path.Combine(_assemblyPaths.Sdk, configurationPath, "mono.js");
            var monoJsDestination = Path.Combine(_configuration.OutputPath, "mono.js");
            var monoJs = File.ReadAllText(monoJsSource);
            monoJs = monoJs.Replace("this.mono_wasm_runtime_is_ready=true;debugger", "this.mono_wasm_runtime_is_ready=true;");
            monoJs = monoJs.Replace(
                "  			this.mono_wasm_runtime_is_ready = true;\n" +
                "  			debugger;\n",
                "  			this.mono_wasm_runtime_is_ready = true;\n");

            File.WriteAllText(monoJsDestination, monoJs);

            _artifactsEmbedder.Perform();

            _assemblies.SkippedImports.ForEach(_ => Console.WriteLine($"  Skipped imports for '{_.File}' with reason '{_.Reason}'"));
        }
    }
}
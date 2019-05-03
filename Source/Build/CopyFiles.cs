/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.IO;
using Dolittle.Build;

namespace Dolittle.Interaction.WebAssembly.Build
{
    /// <summary>
    /// Represents an <see cref="ICanPerformPostBuildTask"/> that deals with copying files
    /// </summary>
    public class CopyFiles : ICanPerformPostBuildTask
    {
        readonly Configuration _configuration;
        readonly Assemblies _assemblies;
        private readonly AssemblyPaths _assemblyPaths;
        private readonly IBuildMessages _buildMessages;

        /// <summary>
        /// Initializes a new instance of <see cref="CopyFiles"/>
        /// </summary>
        /// <param name="configuration"><see cref="Configuration"/> to use</param>
        /// <param name="assemblyPaths"><see cref="AssemblyPaths"/> to use</param>
        /// <param name="assemblies"><see cref="Assemblies"/> for handling what assemblies is part of the deployment</param>
        /// <param name="buildMessages"><see cref="IBuildMessages"/> for build messages</param>
        public CopyFiles(
            Configuration configuration,
            AssemblyPaths assemblyPaths,
            Assemblies assemblies,
            IBuildMessages buildMessages)
        {
            _configuration = configuration;
            _assemblyPaths = assemblyPaths;
            _assemblies = assemblies;
            _buildMessages = buildMessages;
        }

        /// <inheritdoc/>
        public string Message => "Copying files for output";

        /// <inheritdoc/>
        public void Perform()
        {
            var managedFilesFileCopier = new FileCopier(_buildMessages, _configuration.ManagedOutputPath);
            var staticFilesFileCopier = new FileCopier(_buildMessages, _configuration.OutputPath);

            managedFilesFileCopier.Copy(_assemblies.AllImportedAssemblyPaths);
            if (!_configuration.IsRelease) managedFilesFileCopier.Copy(_assemblies.AllImportedAssemblyDebugSymbolPaths);

            var configurationPath = _configuration.IsRelease? "release": "debug";

            staticFilesFileCopier.Copy(new []
            {
                Path.Combine(_assemblyPaths.Sdk, configurationPath, "mono.js"),
                Path.Combine(_assemblyPaths.Sdk, configurationPath, "mono.wasm")
            });

            if (!_configuration.IsRelease)
            {
                staticFilesFileCopier.Copy(new []
                {
                    Path.Combine(_assemblyPaths.Sdk, "debug", "mono.wasm.map"),
                    Path.Combine(_assemblyPaths.Sdk, "debug", "mono.wast")
                });
            }
        }
    }
}
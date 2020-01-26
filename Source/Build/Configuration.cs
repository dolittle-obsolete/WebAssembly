// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using Dolittle.Configuration;

namespace Dolittle.Interaction.WebAssembly.Build
{
    /// <summary>
    /// Represents the configuration for the packager.
    /// </summary>
    public class Configuration : IConfigurationObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        /// <param name="sdkRoot">The path to the root of the WASM Mono SDK.</param>
        /// <param name="outputPath">The path to where to output files.</param>
        /// <param name="boundedContextFilePath">The path to the bounded-context.json file.</param>
        /// <param name="isRelease">Boolean indicating whether or not this is a release build or not.</param>
        /// <param name="dolittleFolder">The path to the .dolittle folder.</param>
        public Configuration(
            string sdkRoot,
            string outputPath,
            string boundedContextFilePath,
            bool isRelease,
            string dolittleFolder)
        {
            SdkRoot = sdkRoot;
            OutputPath = outputPath;
            BoundedContextFilePath = boundedContextFilePath;
            IsRelease = isRelease;
            DolittleFolder = dolittleFolder;

            ManagedOutputPath = Path.Combine(OutputPath, "managed");

            if (!Directory.Exists(OutputPath)) Directory.CreateDirectory(OutputPath);
            if (!Directory.Exists(ManagedOutputPath)) Directory.CreateDirectory(ManagedOutputPath);
        }

        /// <summary>
        /// Gets the path to the root of the sdk.
        /// </summary>
        public string SdkRoot { get; }

        /// <summary>
        /// Gets the output path to be used.
        /// </summary>
        public string OutputPath { get; }

        /// <summary>
        /// Gets the output path for the managed components.
        /// </summary>
        public string ManagedOutputPath { get; }

        /// <summary>
        /// Gets the path to the 'bounded-context.json' file.
        /// </summary>
        public string BoundedContextFilePath { get; }

        /// <summary>
        /// Gets a value indicating whether or not this is a release build.
        /// </summary>
        public bool IsRelease { get; }

        /// <summary>
        /// Gets the path to the .dolittle folder.
        /// </summary>
        public string DolittleFolder { get; }
    }
}

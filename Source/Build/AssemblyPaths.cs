// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using Dolittle.Build;
using Dolittle.Lifecycle;

namespace Dolittle.Interaction.WebAssembly.Build
{
    /// <summary>
    /// Represents something that knows about all the assembly paths.
    /// </summary>
    [Singleton]
    public class AssemblyPaths
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyPaths"/> class.
        /// </summary>
        /// <param name="configuration">The current configuration.</param>
        /// <param name="buildTarget">The current <see cref="BuildTarget"/>.</param>
        public AssemblyPaths(Configuration configuration, BuildTarget buildTarget)
        {
            Sdk = configuration.SdkRoot;
            Bcl = Path.Combine(Sdk, "wasm-bcl", "wasm");
            Framework = Path.Combine(Sdk, "framework");
            Facades = Path.Combine(Bcl, "Facades");
            Root = Path.GetDirectoryName(buildTarget.TargetAssemblyPath);
            SearchPaths = new string[]
            {
                Sdk,
                Bcl,
                Framework,
                Facades,
                Root
            };
        }

        /// <summary>
        /// Gets the path to the Sdk.
        /// </summary>
        public string Sdk { get; }

        /// <summary>
        /// Gets the path to the base class library inside the Sdk.
        /// </summary>
        public string Bcl { get; }

        /// <summary>
        /// Gets the path to the framework inside the Sdk.
        /// </summary>
        public string Framework { get; }

        /// <summary>
        /// Gets the path to the facades inside the Sdk.
        /// </summary>
        public string Facades { get; }

        /// <summary>
        /// Gets the root path.
        /// </summary>
        public string Root { get; }

        /// <summary>
        /// Gets all the search paths used.
        /// </summary>
        public IEnumerable<string> SearchPaths { get; }

        /// <summary>
        /// Find the best match for a given path.
        /// </summary>
        /// <param name="path">Path to find for.</param>
        /// <returns>Full path to the best match of the file. If it was not found in any of the paths, we're assuming the path given is the best.</returns>
        public string FindBestMatchFor(string path)
        {
            var filename = Path.GetFileName(path);
            foreach (var searchPath in SearchPaths)
            {
                var pathToFile = Path.Combine(searchPath, filename);
                if (File.Exists(pathToFile)) return pathToFile;
            }

            return path;
        }
    }
}
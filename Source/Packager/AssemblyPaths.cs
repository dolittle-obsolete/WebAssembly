using System.Collections.Generic;
using System.IO;

namespace Dolittle.WebAssembly.Packager
{
    /// <summary>
    /// Represents something that knows about all the assembly paths
    /// </summary>
    public class AssemblyPaths
    {
        /// <summary>
        /// Initialize a new instance of <see cref="AssemblyPaths"/>
        /// </summary>
        /// <param name="sdkPath">Base path for the WASM SDK</param>
        /// <param name="root">The root path generating from - typically your bin/release or bin/debug</param>
        public AssemblyPaths(string sdkPath, string root)
        {
            Sdk = sdkPath;
            Bcl = Path.Combine(Sdk, "wasm-bcl", "wasm");
            Framework = Path.Combine(Sdk, "framework");
            Facades = Path.Combine(Bcl, "Facades");
            Root = root;
            SearchPaths = new string[] {
                Sdk,
                Bcl,
                Framework,
                Facades,
                Root
            };
        }

        /// <summary>
        /// Find the best match for a given path
        /// </summary>
        /// <param name="path">Path to find for</param>
        /// <returns>Full path to the best match of the file. If it was not found in any of the paths, we're assuming the path given is the best.</returns>
        public string FindBestMatchFor(string path)
        {
            var filename = Path.GetFileName(path);
            foreach( var searchPath in SearchPaths ) 
            {
                var pathToFile = Path.Combine(searchPath, filename);
                if( File.Exists(pathToFile) ) return pathToFile;
            }
            return path;
        }

        /// <summary>
        /// Gets the path to the Sdk
        /// </summary>
        public string Sdk { get; }

        /// <summary>
        /// Gets the path to the base class library inside the Sdk
        /// </summary>
        public string Bcl { get; }

        /// <summary>
        /// Gets the path to the framework inside the Sdk
        /// </summary>
        public string Framework { get; }

        /// <summary>
        /// Gets the path to the facades inside the Sdk
        /// </summary>
        public string Facades { get; }

        /// <summary>
        /// Gets the root path
        /// </summary>
        public string Root { get; }

        /// <summary>
        /// Gets all the search paths used
        /// </summary>
        public IEnumerable<string> SearchPaths { get; }

    }
}
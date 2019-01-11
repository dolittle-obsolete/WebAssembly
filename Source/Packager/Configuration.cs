/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.IO;

namespace Dolittle.Interaction.WebAssembly.Packager
{
    /// <summary>
    /// Represents the configuration for the packager
    /// </summary>
    public class Configuration
    {

        /// <summary>
        /// Initalizes a new instance of <see cref="Configuration"/>
        /// </summary>
        /// <param name="arguments">Command line arguments</param>
        public Configuration(string[] arguments)
        {
            EntryAssemblyPath = arguments[0];
            SdkRoot = arguments[1];
            OutputPath = arguments[2];
            BoundedContextFilePath = arguments[3];

            ManagedOutputPath = Path.Combine(OutputPath, "managed");

            if (!Directory.Exists(OutputPath)) Directory.CreateDirectory(OutputPath);
            if (!Directory.Exists(ManagedOutputPath)) Directory.CreateDirectory(ManagedOutputPath);
        }

        /// <summary>
        /// Get the path to the root of the sdk
        /// </summary>
        public string SdkRoot { get; }

        /// <summary>
        /// Gets the path to the entry assembly
        /// </summary>
        public string EntryAssemblyPath { get; }

        /// <summary>
        /// Gets the output path to be used
        /// </summary>
        public string OutputPath { get; }

        /// <summary>
        /// Gets the output path for the managed components
        /// </summary>
        public string ManagedOutputPath { get; }


        /// <summary>
        /// Gets the path to the 'bounded-context.json' file
        /// </summary>
        public string BoundedContextFilePath { get; }
    }
}

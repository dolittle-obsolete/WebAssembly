/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Reflection;

namespace Dolittle.WebAssembly.Packager
{
    /// <summary>
    /// Represents details about an <see cref="Assembly"/>
    /// </summary>
    public class AssemblyDetails
    {
        /// <summary>
        /// Gets the name of the <see cref="Assembly"/>
        /// </summary>
        public string Name;

        /// <summary>
        /// Gets the filename of the <see cref="Assembly"/>
        /// </summary>
        public string Filename;

        /// <summary>
        /// Gets the source path to the <see cref="Assembly"/>
        /// </summary>
        public string SourcePath;

        // Path of .bc file
        public string bc_path;

        // Path in appdir
        public string app_path;

        // Linker input path
        public string linkin_path;
        // Linker output path
        public string linkout_path;
    }

}
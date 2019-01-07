/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dolittle.Interaction.WebAssembly.Packager
{
    /// <summary>
    /// Represents a system that is capable of copying files
    /// </summary>
    public class FileCopier
    {
        readonly string _destinationPath;

        /// <summary>
        /// Initializes a new instance of <see cref="FileCopier"/>
        /// </summary>
        /// <param name="destinationPath">Destination path the copier is for</param>
        public FileCopier(string destinationPath)
        {
            _destinationPath = destinationPath;
        }


        /// <summary>
        /// Copy files
        /// </summary>
        /// <param name="files">Params of enumerable of files</param>
        public void Copy(params IEnumerable<string>[] files)
        {
            foreach( var file in files.SelectMany(_ => _).Distinct() ) 
            {
                var filename = Path.GetFileName(file);
                var destination = Path.Combine(
                    _destinationPath,
                    filename
                );

                Console.WriteLine($"Copy '{filename}' (Source: {Path.GetDirectoryName(file)}) to '{destination}'");
                File.Copy(file, destination, true);
            }
        }
    }
}

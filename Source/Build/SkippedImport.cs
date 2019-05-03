/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Concepts;

namespace Dolittle.Interaction.WebAssembly.Build
{
    /// <summary>
    /// Represents a skipped import
    /// </summary>
    public class SkippedImport : Value<SkippedImport>
    {
        /// <summary>
        /// Initialzies a new instance of <see cref="SkippedImport"/>
        /// </summary>
        /// <param name="file">File that was skipped</param>
        /// <param name="reason">Reason for why it was skipped</param>
        public SkippedImport(string file, string reason)
        {
            File = file;
            Reason = reason;
        }

        /// <summary>
        /// Gets or sets the file that was skipped
        /// </summary>
        public string File { get; }

        /// <summary>
        /// Gets or sets the reason for why it was skipped
        /// </summary>
        public string Reason { get; }
    }
}
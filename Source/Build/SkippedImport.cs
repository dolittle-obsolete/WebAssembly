// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Concepts;

namespace Dolittle.Interaction.WebAssembly.Build
{
    /// <summary>
    /// Represents a skipped import.
    /// </summary>
    public class SkippedImport : Value<SkippedImport>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkippedImport"/> class.
        /// </summary>
        /// <param name="file">File that was skipped.</param>
        /// <param name="reason">Reason for why it was skipped.</param>
        public SkippedImport(string file, string reason)
        {
            File = file;
            Reason = reason;
        }

        /// <summary>
        /// Gets the file that was skipped.
        /// </summary>
        public string File { get; }

        /// <summary>
        /// Gets the reason for why it was skipped.
        /// </summary>
        public string Reason { get; }
    }
}
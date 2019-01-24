/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Microsoft.Extensions.DependencyModel;

namespace Dolittle.Interaction.WebAssembly.Booting
{
    /// <summary>
    /// EqualityComparer for <see cref="Library"/>
    /// </summary>
    public class LibraryComparer : IEqualityComparer<Library>
    {
        /// <inheritdoc/>
        public bool Equals(Library x, Library y)
        {
            return x.Name.Equals(y.Name);
        }

        /// <inheritdoc/>
        public int GetHashCode(Library obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
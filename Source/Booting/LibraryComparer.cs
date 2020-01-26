// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyModel;

namespace Dolittle.Interaction.WebAssembly.Booting
{
    /// <summary>
    /// EqualityComparer for <see cref="Library"/>.
    /// </summary>
    public class LibraryComparer : IEqualityComparer<Library>
    {
        /// <inheritdoc/>
        public bool Equals(Library x, Library y)
        {
            return x.Name.Equals(y.Name, StringComparison.InvariantCulture);
        }

        /// <inheritdoc/>
        public int GetHashCode(Library obj)
        {
            return obj.Name.GetHashCode(StringComparison.InvariantCulture);
        }
    }
}
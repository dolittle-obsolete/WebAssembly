// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Dolittle.Serialization.Json;
using Newtonsoft.Json;

namespace Dolittle.Runtime.Events.WebAssembly.Dev
{
    /// <summary>
    /// The provider that registers the PropertyBag JsonConverter.
    /// </summary>
    public class PropertyBagSerializerProvider : ICanProvideConverters
    {
        /// <inheritdoc/>
        public IEnumerable<JsonConverter> Provide()
        {
            return new[]
            {
                new PropertyBagSerializer()
            };
        }
    }
}
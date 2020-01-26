// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Dolittle.ResourceTypes;

namespace Dolittle.ReadModels.MongoDB.WebAssembly
{
    /// <summary>
    /// Represents a definition of a resource type for MiniMongo MongoDB ReadModels.
    /// </summary>
    public class ResourceTypeRepresentation : IRepresentAResourceType
    {
        static readonly IDictionary<Type, Type> _bindings = new Dictionary<Type, Type>
        {
            { typeof(IAsyncReadModelRepositoryFor<>), typeof(AsyncReadModelRepositoryFor<>) }
        };

        /// <inheritdoc/>
        public ResourceType Type => "readModels";

        /// <inheritdoc/>
        public ResourceTypeImplementation ImplementationName => "MiniMongo";

        /// <inheritdoc/>
        public Type ConfigurationObjectType => typeof(Configuration);

        /// <inheritdoc/>
        public IDictionary<Type, Type> Bindings => _bindings;
    }
}
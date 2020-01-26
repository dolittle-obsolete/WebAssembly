// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Dolittle.ResourceTypes;
using Dolittle.Runtime.Events.Store.WebAssembly.Dev;

namespace Dolittle.Runtime.Events.WebAssembly.Dev
{
    /// <inheritdoc/>
    public class ResourceTypeRepresentation : IRepresentAResourceType
    {
        static readonly IDictionary<Type, Type> _bindings = new Dictionary<Type, Type>
        {
            { typeof(Store.IEventStore), typeof(Store.WebAssembly.Dev.EventStore) },
            { typeof(Relativity.IGeodesics), typeof(Relativity.WebAssembly.Dev.Geodesics) },
            { typeof(Processing.IEventProcessorOffsetRepository), typeof(Processing.WebAssembly.Dev.EventProcessorOffsetRepository) }
        };

        /// <inheritdoc/>
        public ResourceType Type => "eventStore";

        /// <inheritdoc/>
        public ResourceTypeImplementation ImplementationName => "WebAssemblyDev";

        /// <inheritdoc/>
        public Type ConfigurationObjectType => typeof(EventStoreConfiguration);

        /// <inheritdoc/>
        public IDictionary<Type, Type> Bindings => _bindings;
    }
}
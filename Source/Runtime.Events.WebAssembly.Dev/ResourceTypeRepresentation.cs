/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/
 
using System;
using System.Collections.Generic;
using Dolittle.ResourceTypes;
using Dolittle.Runtime.Events.Store.WebAssembly.Dev;

namespace Dolittle.Runtime.Events.WebAssembly.Dev
{
    /// <inheritdoc/>
    public class ResourceTypeRepresentation : IRepresentAResourceType
    {
        static IDictionary<Type, Type> _bindings = new Dictionary<Type, Type>
        {
            {typeof(Dolittle.Runtime.Events.Store.IEventStore), typeof(Dolittle.Runtime.Events.Store.WebAssembly.Dev.EventStore)},
            {typeof(Dolittle.Runtime.Events.Relativity.IGeodesics), typeof(Dolittle.Runtime.Events.Relativity.WebAssembly.Dev.Geodesics)},
            {typeof(Dolittle.Runtime.Events.Processing.IEventProcessorOffsetRepository), typeof(Dolittle.Runtime.Events.Processing.WebAssembly.Dev.EventProcessorOffsetRepository)}
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
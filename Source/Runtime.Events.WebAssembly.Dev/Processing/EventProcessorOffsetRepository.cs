// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using Dolittle.Collections;
using Dolittle.Interaction.WebAssembly.Interop;
using Dolittle.Logging;
using Dolittle.Runtime.Events.Store;

namespace Dolittle.Runtime.Events.Processing.WebAssembly.Dev
{
    /// <summary>
    /// In-Memory Implementation of <see cref="IEventProcessorOffsetRepository" />
    /// For testing purposes only.
    /// </summary>
    public class EventProcessorOffsetRepository : IEventProcessorOffsetRepository
    {
        const string _globalObject = "window._eventStore.eventProcessorOffsetRepository";
        readonly IJSRuntime _jsRuntime;
        readonly ILogger _logger;
        readonly ConcurrentDictionary<EventProcessorId, CommittedEventVersion> _lastProcessed;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventProcessorOffsetRepository"/> class.
        /// </summary>
        /// <param name="jSRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public EventProcessorOffsetRepository(
            IJSRuntime jSRuntime,
            ILogger logger)
        {
            _jsRuntime = jSRuntime;
            _logger = logger;
            _lastProcessed = new ConcurrentDictionary<EventProcessorId, CommittedEventVersion>();

            var result = _jsRuntime.Invoke<IEnumerable<EventProcessorOffset>>($"{_globalObject}.load").Result;
            result.ForEach(_ => _lastProcessed.AddOrUpdate(_.EventProcessorId, _.Content, (id, v) => _.Content));
        }

        /// <inheritdoc />
        public CommittedEventVersion Get(EventProcessorId eventProcessorId)
        {
            return _lastProcessed.TryGetValue(eventProcessorId, out CommittedEventVersion lastProcessedVersion) ? lastProcessedVersion : CommittedEventVersion.None;
        }

        /// <inheritdoc />
        public void Set(EventProcessorId eventProcessorId, CommittedEventVersion committedEventVersion)
        {
            _lastProcessed.AddOrUpdate(eventProcessorId, committedEventVersion, (id, v) => committedEventVersion);
            _logger.Information($"Saving event processor offset");
            _jsRuntime.Invoke($"{_globalObject}.save", eventProcessorId, committedEventVersion);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }

#pragma warning disable CA1812
        class EventProcessorOffset
        {
            public EventProcessorId EventProcessorId;
            public CommittedEventVersion Content;

            public EventProcessorOffset(EventProcessorId eventProcessorId, CommittedEventVersion content)
            {
                EventProcessorId = eventProcessorId;
                Content = content;
            }
        }
    }
}
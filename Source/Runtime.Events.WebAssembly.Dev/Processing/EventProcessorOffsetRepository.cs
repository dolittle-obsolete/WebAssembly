/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/


namespace Dolittle.Runtime.Events.Processing.WebAssembly.Dev
{
    using Dolittle.Collections;
    using Dolittle.Interaction.WebAssembly.Interop;
    using Dolittle.Logging;
    using Dolittle.Runtime.Events.Processing;
    using Dolittle.Runtime.Events.Store;
    using Dolittle.Serialization.Json;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// In-Memory Implementation of <see cref="IEventProcessorOffsetRepository" />
    /// For testing purposes only
    /// </summary>
    public class EventProcessorOffsetRepository : IEventProcessorOffsetRepository
    {
        const string _globalObject = "window._eventStore.eventProcessorOffsetRepository";
        readonly IJSRuntime _jsRuntime;
        readonly ILogger _logger;
        ConcurrentDictionary<EventProcessorId,CommittedEventVersion> _lastProcessed;

        class EventProcessorOffset
        {
            public EventProcessorOffset(EventProcessorId eventProcessorId, CommittedEventVersion content)
            {
                EventProcessorId = eventProcessorId;
                Content = content;
            }
            public EventProcessorId EventProcessorId;
            public CommittedEventVersion Content;
        }

        /// <summary>
        /// Instantiates an instance of <see cref="EventProcessorOffsetRepository" />
        /// </summary>
        public EventProcessorOffsetRepository(IJSRuntime jSRuntime, ISerializer serializer, ILogger logger)
        {
            _jsRuntime = jSRuntime;
            _logger = logger;
            _lastProcessed = new ConcurrentDictionary<EventProcessorId, CommittedEventVersion>();

            var result = _jsRuntime.Invoke<IEnumerable<EventProcessorOffset>>($"{_globalObject}.load").Result;
            result.ForEach(_ => {
                _lastProcessed.AddOrUpdate(_.EventProcessorId,_.Content,(id,v) => _.Content);
            });
        }

        /// <inheritdoc />
        public CommittedEventVersion Get(EventProcessorId eventProcessorId)
        {
            CommittedEventVersion lastProcessedVersion;
            return _lastProcessed.TryGetValue(eventProcessorId, out lastProcessedVersion) ? lastProcessedVersion : CommittedEventVersion.None;
        }

        /// <inheritdoc />
        public void Set(EventProcessorId eventProcessorId, CommittedEventVersion committedEventVersion)
        {
            _lastProcessed.AddOrUpdate(eventProcessorId,committedEventVersion,(id,v) => committedEventVersion);
            _logger.Information($"Saving event processor offset");
            _jsRuntime.Invoke($"{_globalObject}.save", eventProcessorId, committedEventVersion);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~EventProcessorOffsetRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
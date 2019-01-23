/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/


namespace Dolittle.Runtime.Events.Relativity.WebAssembly.Dev
{
    using Dolittle.Runtime.Events.Processing;
    using Dolittle.Runtime.Events.Processing.WebAssembly.Dev;
    using Dolittle.Runtime.Events.Store;
    using System.Collections.Concurrent;

    /// <summary>
    /// In-Memory Implementation of <see cref="IEventProcessorOffsetRepository" />
    /// For testing purposes only
    /// </summary>
    public class Geodesics : IGeodesics
    {
        ConcurrentDictionary<EventHorizonKey,ulong> _lastProcessed;

        /// <summary>
        /// Instantiates an instance of <see cref="EventProcessorOffsetRepository" />
        /// </summary>
        public Geodesics()
        {
            _lastProcessed = new ConcurrentDictionary<EventHorizonKey, ulong>();
        }

        /// <inheritdoc />
        public ulong GetOffset(EventHorizonKey key)
        {
            ulong offset;
            return _lastProcessed.TryGetValue(key, out offset) ? offset : 0;
        }

        /// <inheritdoc />
        public void SetOffset(EventHorizonKey key, ulong offset)
        {
            _lastProcessed.AddOrUpdate(key,offset,(id,v) => offset);
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
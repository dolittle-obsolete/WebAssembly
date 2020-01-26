// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using Dolittle.Runtime.Events.Processing;

namespace Dolittle.Runtime.Events.Relativity.WebAssembly.Dev
{
    /// <summary>
    /// In-Memory Implementation of <see cref="IEventProcessorOffsetRepository" />
    /// For testing purposes only.
    /// </summary>
    public class Geodesics : IGeodesics
    {
        readonly ConcurrentDictionary<EventHorizonKey, ulong> _lastProcessed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Geodesics"/> class.
        /// </summary>
        public Geodesics()
        {
            _lastProcessed = new ConcurrentDictionary<EventHorizonKey, ulong>();
        }

        /// <inheritdoc />
        public ulong GetOffset(EventHorizonKey key)
        {
            return _lastProcessed.TryGetValue(key, out ulong offset) ? offset : 0;
        }

        /// <inheritdoc />
        public void SetOffset(EventHorizonKey key, ulong offset)
        {
            _lastProcessed.AddOrUpdate(key, offset, (_, __) => offset);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}
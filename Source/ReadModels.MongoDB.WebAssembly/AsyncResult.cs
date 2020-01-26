// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Dolittle.ReadModels.MongoDB.WebAssembly
{
    /// <summary>
    /// Represents the async result from working with Mongo.
    /// </summary>
    /// <typeparam name="T">Type of result document.</typeparam>
    public class AsyncResult<T> : IAsyncCursor<T>
    {
        bool _canMove = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncResult{T}"/> class.
        /// </summary>
        /// <param name="results">The results.</param>
        public AsyncResult(IEnumerable<T> results)
        {
            Current = results;
        }

        /// <inheritdoc/>
        public IEnumerable<T> Current { get; }

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        public bool MoveNext(CancellationToken cancellationToken = default)
        {
            var canMove = _canMove;
            _canMove = false;
            return canMove;
        }

        /// <inheritdoc/>
        public Task<bool> MoveNextAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(MoveNext(cancellationToken));
        }
    }
}
/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Dolittle.ReadModels.MongoDB.WebAssembly
{
    /// <summary>
    /// 
    /// </summary>
    public class AsyncResult<T> : IAsyncCursor<T>
    {
        IEnumerable<T> _results;
        bool _canMove = true;

        /// <summary>
        /// 
        /// </summary>
        public AsyncResult(IEnumerable<T> result)
        {
            _results = result;
        }

        /// <inheritdoc/>
        public IEnumerable<T> Current => _results;

        /// <inheritdoc/>
        public void Dispose()
        {
            
        }

        /// <inheritdoc/>
        public bool MoveNext(CancellationToken cancellationToken = default(CancellationToken))
        {
            var canMove = _canMove;
            _canMove = false;
            return canMove;
        }

        /// <inheritdoc/>
        public Task<bool> MoveNextAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(MoveNext(cancellationToken));
        }
    }
}
// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;
using Dolittle.Logging;
using MongoDB.Driver;

namespace Dolittle.ReadModels.MongoDB.WebAssembly
{
    /// <summary>
    /// Represents an implementation of <see cref="IReadModelRepositoryFor{T}"/> for MongoDB.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IReadModel"/>.</typeparam>.
    public class AsyncReadModelRepositoryFor<T> : IAsyncReadModelRepositoryFor<T>
        where T : IReadModel
    {
        readonly string _collectionName = typeof(T).Name;
        readonly IMongoCollection<T> _collection;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncReadModelRepositoryFor{T}"/> class.
        /// </summary>
        /// <param name="database"><see cref="IMongoDatabase"/> to use.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public AsyncReadModelRepositoryFor(IMongoDatabase database, ILogger logger)
        {
            _collection = database.GetCollection<T>(_collectionName);
            _logger = logger;
        }

        /// <inheritdoc/>
        public IQueryable<T> Query => null;

        /// <inheritdoc/>
        public async Task Delete(T readModel)
        {
            var objectId = readModel.GetObjectIdFrom();
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", objectId)).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<T> GetById(object id)
        {
            var objectId = id.GetIdAsBsonValue();

            var result = await _collection.FindAsync(Builders<T>.Filter.Eq("_id", objectId)).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task Insert(T readModel)
        {
            _logger.Information("Insert document");
            await _collection.InsertOneAsync(readModel).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task Update(T readModel)
        {
            var id = readModel.GetObjectIdFrom();

            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.ReplaceOneAsync(filter, readModel, new UpdateOptions() { IsUpsert = true }).ConfigureAwait(false);
        }
    }
}
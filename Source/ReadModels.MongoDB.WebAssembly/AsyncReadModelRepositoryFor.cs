/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dolittle.Concepts;
using Dolittle.Logging;
using Dolittle.ReadModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Dolittle.ReadModels.MongoDB.WebAssembly
{
    /// <summary>
    /// Represents an implementation of <see cref="IReadModelRepositoryFor{T}"/> for MongoDB
    /// </summary>
    public class AsyncReadModelRepositoryFor<T> : IAsyncReadModelRepositoryFor<T> where T : Dolittle.ReadModels.IReadModel
    {
        readonly string _collectionName = typeof(T).Name;
        readonly IMongoCollection<T> _collection;
        readonly ILogger _logger;


        /// <summary>
        /// Initializes a new instance of <see cref="AsyncReadModelRepositoryFor{T}"/>
        /// </summary>
        /// <param name="database"><see cref="IMongoDatabase"/> to use</param>
        /// <param name="logger"><see cref="ILogger"/> for logging</param>
        public AsyncReadModelRepositoryFor(IMongoDatabase database, ILogger logger)
        {
            _collection = database.GetCollection<T>(_collectionName);
            _logger = logger;
        }

        /// <inheritdoc/>
        public IQueryable<T> Query => throw new NotImplementedException();

        /// <inheritdoc/>
        public async Task Delete(T readModel)
        {
            var objectId = GetObjectIdFrom(readModel);
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", objectId));
        }

        /// <inheritdoc/>
        public async Task<T> GetById(object id)
        {
            var objectId = GetIdAsBsonValue(id);

            var result = await _collection.FindAsync(Builders<T>.Filter.Eq("_id", objectId));
            return result.FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task Insert(T readModel)
        {
            _logger.Information("Insert document");
            await _collection.InsertOneAsync(readModel);
        }

        /// <inheritdoc/>
        public async Task Update(T readModel)
        {
            var id = GetObjectIdFrom(readModel);

            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.ReplaceOneAsync(filter, readModel, new UpdateOptions() { IsUpsert = true });
        }

        BsonValue GetObjectIdFrom(T entity)
        {
            var propInfo = GetIdProperty(entity);
            object id = propInfo.GetValue(entity);

            return GetIdAsBsonValue(id);
        }

        BsonValue GetIdAsBsonValue(object id)
        {
            var idVal = id;
            if (id.IsConcept())
                idVal = id.GetConceptValue();

            var idAsValue = BsonValue.Create(idVal);
            return idAsValue;
        }

        PropertyInfo GetIdProperty(T entity)
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.Name.ToLowerInvariant()== "id").First();
        }
    }
}
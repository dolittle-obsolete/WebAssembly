/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Threading;
using System.Threading.Tasks;
using Dolittle.ResourceTypes.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Dolittle.ReadModels.MongoDB.WebAssembly
{
    /// <summary>
    /// Represents an implementation of <see cref="IMongoDatabase"/> for working with MiniMongo
    /// </summary>
    public class MiniMongoDatabase : IMongoDatabase
    {
        readonly Configuration _configuration;

        /// <summary>
        /// Initializes a new instance of <see cref="MiniMongoDatabase"/>
        /// </summary>
        /// <param name="configurationFor"></param>
        public MiniMongoDatabase(IConfigurationFor<Configuration> configurationFor)
        {
            _configuration = configurationFor.Instance;
        }

        /// <inheritdoc/>
        public IMongoClient Client => throw new System.NotImplementedException();

        /// <inheritdoc/>
        public DatabaseNamespace DatabaseNamespace => throw new System.NotImplementedException();

        /// <inheritdoc/>
        public MongoDatabaseSettings Settings => throw new System.NotImplementedException();

        /// <inheritdoc/>
        public void CreateCollection(string name, CreateCollectionOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task CreateCollectionAsync(string name, CreateCollectionOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void CreateView<TDocument, TResult>(string viewName, string viewOn, PipelineDefinition<TDocument, TResult> pipeline, CreateViewOptions<TDocument> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task CreateViewAsync<TDocument, TResult>(string viewName, string viewOn, PipelineDefinition<TDocument, TResult> pipeline, CreateViewOptions<TDocument> options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void DropCollection(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task DropCollectionAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IMongoCollection<TDocument> GetCollection<TDocument>(string name, MongoCollectionSettings settings = null)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IAsyncCursor<BsonDocument> ListCollections(ListCollectionsOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IAsyncCursor<BsonDocument>> ListCollectionsAsync(ListCollectionsOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void RenameCollection(string oldName, string newName, RenameCollectionOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task RenameCollectionAsync(string oldName, string newName, RenameCollectionOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public TResult RunCommand<TResult>(Command<TResult> command, ReadPreference readPreference = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<TResult> RunCommandAsync<TResult>(Command<TResult> command, ReadPreference readPreference = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IMongoDatabase WithReadConcern(ReadConcern readConcern)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IMongoDatabase WithReadPreference(ReadPreference readPreference)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IMongoDatabase WithWriteConcern(WriteConcern writeConcern)
        {
            throw new System.NotImplementedException();
        }
    }
}
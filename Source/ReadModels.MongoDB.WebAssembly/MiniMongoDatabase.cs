// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dolittle.Interaction.WebAssembly.Interop;
using Dolittle.Logging;
using Dolittle.ResourceTypes.Configuration;
using Dolittle.Serialization.Json;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Dolittle.ReadModels.MongoDB.WebAssembly
{
    /// <summary>
    /// Represents an implementation of <see cref="IMongoDatabase"/> for working with MiniMongo.
    /// </summary>
    public class MiniMongoDatabase : IMongoDatabase
    {
        internal const string _globalObject = "window._mongoDB";

        readonly IConfigurationFor<Configuration> _configuration;
        readonly IJSRuntime _jsRuntime;
        readonly ISerializer _serializer;
        readonly ILogger _logger;
        readonly Dictionary<string, object> _collections = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MiniMongoDatabase"/> class.
        /// </summary>
        /// <param name="configurationFor"><see cref="Configuration"/>.</param>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="serializer">JSON <see cref="ISerializer"/>.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public MiniMongoDatabase(
            IConfigurationFor<Configuration> configurationFor,
            IJSRuntime jsRuntime,
            ISerializer serializer,
            ILogger logger)
        {
            _configuration = configurationFor;
            _jsRuntime = jsRuntime;
            _serializer = serializer;
            _logger = logger;
        }

        /// <inheritdoc/>
        public IMongoCollection<TDocument> GetCollection<TDocument>(string name, MongoCollectionSettings settings = null)
        {
            if (_collections.ContainsKey(name)) return (IMongoCollection<TDocument>)_collections[name];
            var collection = new MiniMongoCollection<TDocument>(name, _configuration, _jsRuntime, _serializer, _logger);
            _collections[name] = collection;

            _jsRuntime.Invoke($"{_globalObject}.database.addCollection", name);
            return collection;
        }

#pragma warning disable SA1201, DL0008, RCS1079, SA1124

        /// <inheritdoc/>
        public IMongoClient Client => throw new System.NotImplementedException();

        /// <inheritdoc/>
        public DatabaseNamespace DatabaseNamespace => throw new System.NotImplementedException();

        /// <inheritdoc/>
        public MongoDatabaseSettings Settings => throw new System.NotImplementedException();

        /// <inheritdoc/>
        public void CreateCollection(string name, CreateCollectionOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void CreateCollection(IClientSessionHandle session, string name, CreateCollectionOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task CreateCollectionAsync(string name, CreateCollectionOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task CreateCollectionAsync(IClientSessionHandle session, string name, CreateCollectionOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void CreateView<TDocument, TResult>(string viewName, string viewOn, PipelineDefinition<TDocument, TResult> pipeline, CreateViewOptions<TDocument> options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void CreateView<TDocument, TResult>(IClientSessionHandle session, string viewName, string viewOn, PipelineDefinition<TDocument, TResult> pipeline, CreateViewOptions<TDocument> options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task CreateViewAsync<TDocument, TResult>(string viewName, string viewOn, PipelineDefinition<TDocument, TResult> pipeline, CreateViewOptions<TDocument> options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task CreateViewAsync<TDocument, TResult>(IClientSessionHandle session, string viewName, string viewOn, PipelineDefinition<TDocument, TResult> pipeline, CreateViewOptions<TDocument> options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void DropCollection(string name, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void DropCollection(IClientSessionHandle session, string name, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task DropCollectionAsync(string name, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task DropCollectionAsync(IClientSessionHandle session, string name, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IAsyncCursor<string> ListCollectionNames(ListCollectionNamesOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IAsyncCursor<string> ListCollectionNames(IClientSessionHandle session, ListCollectionNamesOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IAsyncCursor<string>> ListCollectionNamesAsync(ListCollectionNamesOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IAsyncCursor<string>> ListCollectionNamesAsync(IClientSessionHandle session, ListCollectionNamesOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IAsyncCursor<BsonDocument> ListCollections(ListCollectionsOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IAsyncCursor<BsonDocument> ListCollections(IClientSessionHandle session, ListCollectionsOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IAsyncCursor<BsonDocument>> ListCollectionsAsync(ListCollectionsOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IAsyncCursor<BsonDocument>> ListCollectionsAsync(IClientSessionHandle session, ListCollectionsOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void RenameCollection(string oldName, string newName, RenameCollectionOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void RenameCollection(IClientSessionHandle session, string oldName, string newName, RenameCollectionOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task RenameCollectionAsync(string oldName, string newName, RenameCollectionOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task RenameCollectionAsync(IClientSessionHandle session, string oldName, string newName, RenameCollectionOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public TResult RunCommand<TResult>(Command<TResult> command, ReadPreference readPreference = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public TResult RunCommand<TResult>(IClientSessionHandle session, Command<TResult> command, ReadPreference readPreference = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<TResult> RunCommandAsync<TResult>(Command<TResult> command, ReadPreference readPreference = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<TResult> RunCommandAsync<TResult>(IClientSessionHandle session, Command<TResult> command, ReadPreference readPreference = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IAsyncCursor<TResult> Watch<TResult>(PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IAsyncCursor<TResult> Watch<TResult>(IClientSessionHandle session, PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IAsyncCursor<TResult>> WatchAsync<TResult>(PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IAsyncCursor<TResult>> WatchAsync<TResult>(IClientSessionHandle session, PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
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
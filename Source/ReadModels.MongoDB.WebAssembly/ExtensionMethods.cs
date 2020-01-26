// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Dolittle.ReadModels.MongoDB.WebAssembly
{
    /// <summary>
    /// General extension methods for MongoDB.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Convert a <see cref="FilterDefinition{T}"/> to Json.
        /// </summary>
        /// <typeparam name="T">Type of document.</typeparam>
        /// <param name="filter"><see cref="FilterDefinition{T}"/> to convert.</param>
        /// <returns>Json representation.</returns>
        public static string ToJson<T>(this FilterDefinition<T> filter)
        {
            var documentSerializer = BsonSerializer.SerializerRegistry.GetSerializer<T>();
            var renderedFilter = filter.Render(documentSerializer, BsonSerializer.SerializerRegistry);
            return renderedFilter.ToJson();
        }
    }
}
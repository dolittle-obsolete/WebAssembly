// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.PropertyBags;
using Dolittle.Runtime.Events.MongoDB;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace Dolittle.Runtime.Events.WebAssembly.Dev
{
    /// <summary>
    /// Represents a Json converter that handles PropertyBags, currently by leveraging (abusing) the MongoDB BSON serializers and strings.
    /// </summary>
    public class PropertyBagSerializer : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PropertyBag);
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var json = reader.Value as string;
            var bson = BsonDocument.Parse(json);
            return PropertyBagBsonSerializer.Deserialize(bson);
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bson = PropertyBagBsonSerializer.Serialize(value as PropertyBag);
            var json = bson.ToJson();
            writer.WriteValue(json);
        }
    }
}
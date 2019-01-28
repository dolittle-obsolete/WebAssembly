
using System;
using System.Collections.Generic;
using Dolittle.Collections;
using Dolittle.PropertyBags;
using Dolittle.Runtime.Events.MongoDB;
using Dolittle.Serialization.Json;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dolittle.Runtime.Events.WebAssembly.Dev
{
    /// <summary>
    /// Represents a Json converter that handles PropertyBags, currently by leveraging (abusing) the MongoDB BSON serializers and strings
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
            var dict = new NullFreeDictionary<string, object>();
            bson.ForEach(_ => {
                var value = BsonTypeMapper.MapToDotNetValue(_.Value);
                dict.Add(_.Name, value);
            });
            return new PropertyBag(dict);
        }
        
        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bson = PropertyBagBsonSerializer.Serialize(value as PropertyBag);
            var json = bson.ToJson();
            writer.WriteValue(json);
        }
    }

    /// <summary>
    /// The provider that registers the PropertyBag JsonConverter
    /// </summary>
    public class PropertyBagSerializerProvider : ICanProvideConverters
    {
        /// <inheritdoc/>
        public IEnumerable<JsonConverter> Provide()
        {
            return new[] {
                new PropertyBagSerializer()
            };
        }
    }
}

using System;
using System.Collections.Generic;
using Dolittle.Collections;
using Dolittle.PropertyBags;
using Dolittle.Runtime.Events.MongoDB;
using Dolittle.Serialization.Json;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#pragma warning disable 1591
namespace Dolittle.Runtime.Events.WebAssembly.Dev
{
    public class PropertyBagSerializer : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PropertyBag);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var json = reader.Value as string;
            var bson = BsonDocument.Parse(json);
            var dict = new NullFreeDictionary<string, object>();
            bson.ForEach(_ => dict.Add(_.Name, _.Value));
            return new PropertyBag(dict);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bson = PropertyBagBsonSerializer.Serialize(value as PropertyBag);
            var json = bson.ToJson();
            writer.WriteValue(json);
        }
    }

    public class PropertyBagSerializerProvider : ICanProvideConverters
    {
        public IEnumerable<JsonConverter> Provide()
        {
            return new[] {
                new PropertyBagSerializer()
            };
        }
    }
}
#pragma warning restore 1591
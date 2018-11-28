using System;
using Dolittle.Logging;
using Dolittle.Serialization.Json;
using WebAssembly;
using Dolittle.Events.Processing;

namespace Basic.MyFeature
{


    public class MyEventProcessors : ICanProcessEvents
    {
        readonly ILogger _logger;
        readonly ISerializer _serializer;

        public MyEventProcessors(ILogger logger, ISerializer serializer)
        {
            _logger = logger;
            _serializer = serializer;
        }

        [EventProcessor("0b519add-ae77-40a9-bc9b-30f014e8a186")]
        public void Handle(MyEvent @event)
        {
            _logger.Information("Event processed");

            var window = (JSObject) WebAssembly.Runtime.GetGlobalObject("window");
            var mongoDb = (JSObject)window.GetObjectProperty("mongoDb");
            var database = (JSObject)mongoDb.GetObjectProperty("database");
            var collection = (JSObject)mongoDb.GetObjectProperty("collection");

            var document = new Animal {
                Species = "Dog",
                Name = Guid.NewGuid().ToString()
            };

            var json = _serializer.ToJson(document);

            // Insert the document
            mongoDb.Invoke("insert", json);
        }
    }
}
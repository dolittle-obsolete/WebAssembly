using System;
using Dolittle.Logging;
using Dolittle.Serialization.Json;
using WebAssembly;
using Dolittle.Events.Processing;
using Dolittle.ReadModels;

namespace Basic.MyFeature
{
    public class MyEventProcessors : ICanProcessEvents
    {
        readonly ILogger _logger;
        readonly IReadModelRepositoryFor<Animal> _repository;

        public MyEventProcessors(ILogger logger, IReadModelRepositoryFor<Animal> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [EventProcessor("0b519add-ae77-40a9-bc9b-30f014e8a186")]
        public void Process(MyEvent @event)
        {
            _logger.Information("Event processed");
            /*
            var document = new Animal {
                Species = "Dog",
                Name = Guid.NewGuid().ToString()
            };

            _repository.Insert(document);*/
        }
    }
}
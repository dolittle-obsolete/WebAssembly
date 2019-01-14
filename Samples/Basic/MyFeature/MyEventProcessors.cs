using System;
using Dolittle.Events.Processing;
using Dolittle.Logging;
using Dolittle.ReadModels;
using Dolittle.Serialization.Json;
using WebAssembly;

namespace Basic.MyFeature
{
    public class MyEventProcessors : ICanProcessEvents
    {
        readonly ILogger _logger;
        readonly IAsyncReadModelRepositoryFor<Animal> _repository;

        public MyEventProcessors(ILogger logger, IAsyncReadModelRepositoryFor<Animal> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [EventProcessor("0b519add-ae77-40a9-bc9b-30f014e8a186")]
        public void Process(MyEvent @event)
        {
            _logger.Information("Event processed");

            try
            { 
                var document = new Animal
                {
                    Species = "Dog",
                    Name = Guid.NewGuid().ToString()
                };

                _repository.Insert(document);
                _logger.Information("Inserted document");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Couldn't insert document ");

            }
        }
    }
}
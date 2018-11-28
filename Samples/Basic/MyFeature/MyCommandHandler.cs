using System;
using Dolittle.Logging;
using Dolittle.Commands.Handling;
using Dolittle.Domain;

namespace Basic.MyFeature
{


    public class MyCommandHandler : ICanHandleCommands
    {
        private readonly ILogger _logger;
        private readonly IAggregateRootRepositoryFor<MyAggregate> _repository;

        public MyCommandHandler(ILogger logger, IAggregateRootRepositoryFor<MyAggregate> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public void Handle(MyCommand command)
        {
            _logger.Information("Handling the command");
            try {

                var aggregate = _repository.Get(Guid.NewGuid());
                aggregate.DoStuff();


            } catch( Exception ex ) {
                _logger.Error(ex,"Couldn't do stuff");
            }
        }
    }
}
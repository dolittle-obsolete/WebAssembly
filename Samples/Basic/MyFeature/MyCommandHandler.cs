using System;
using Dolittle.Commands.Handling;
using Dolittle.Domain;
using Dolittle.Logging;

namespace Basic.MyFeature
{

    public class MyCommandHandler : ICanHandleCommands
    {
        readonly ILogger _logger;
        readonly IAggregateRootRepositoryFor<MyAggregate> _repository;

        public MyCommandHandler(ILogger logger, IAggregateRootRepositoryFor<MyAggregate> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public void Handle(MyCommand command)
        {
            _logger.Information("Handling the command");
            var aggregate = _repository.Get(Guid.NewGuid());
            aggregate.DoStuff();
        }

        public void Handle(DeleteAnimal command)
        {
            _logger.Information("Handling the command");
            var aggregate = _repository.Get(command.Animal);
            aggregate.Delete();
        }
        
    }
}
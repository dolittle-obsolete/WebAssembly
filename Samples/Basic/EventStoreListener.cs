
using System.Linq;
using Dolittle.Logging;
using Dolittle.Runtime.Events.Store;
using Dolittle.Runtime.Events.WebAssembly.Dev;

namespace Basic
{
    public class EventStoreListener : IWantToBeNotifiedWhenEventsAreCommited
    {
        private readonly ILogger _logger;

        public EventStoreListener(ILogger logger)
        {
            _logger = logger;
        }

        public void Handle(CommittedEventStream committedEvents)
        {
            _logger.Information($"Listener was notified of {committedEvents.Events.Count()} events");
        }
    }
}
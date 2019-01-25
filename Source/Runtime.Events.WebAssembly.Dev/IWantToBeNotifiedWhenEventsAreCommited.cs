using Dolittle.Runtime.Events.Store;

namespace Dolittle.Runtime.Events.WebAssembly.Dev
{
    /// <summary>
    /// Represents a type that will be notified whenever events are commited to the event store
    /// </summary>
    public interface IWantToBeNotifiedWhenEventsAreCommited
    {
        /// <summary>
        /// The method that will be called when events are commited to the event store
        /// </summary>
        /// <param name="committedEvents"></param>
        void Handle(CommittedEventStream committedEvents);
    }
}
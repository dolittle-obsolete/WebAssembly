using System;
using System.Collections.Generic;
using Dolittle.Artifacts;
using Dolittle.Logging;
using Dolittle.ResourceTypes;
using Dolittle.Runtime.Events;
using Dolittle.Runtime.Events.Processing;
using Dolittle.Runtime.Events.Relativity;
using Dolittle.Runtime.Events.Store;

namespace Basic
{
    public class EventStoreResourceTypeRepresentation : IRepresentAResourceType
    {

        static IDictionary<Type, Type> _bindings = new Dictionary<Type, Type>
        {
            {typeof(Dolittle.Runtime.Events.Store.IEventStore), typeof(EventStore)},
            {typeof(Dolittle.Runtime.Events.Relativity.IGeodesics), typeof(Geodesics)},
            {typeof(Dolittle.Runtime.Events.Processing.IEventProcessorOffsetRepository), typeof(EventProcessorOffsetRepository)}
        };
        
        public ResourceType Type => "eventStore";
        public ResourceTypeImplementation ImplementationName => "IndexedDB";
        public Type ConfigurationObjectType => typeof(EventStoreConfiguration);
        public IDictionary<Type, Type> Bindings => _bindings;
    }    


    public class EventStoreConfiguration
    {

    }

    public class EventProcessorOffsetRepository : IEventProcessorOffsetRepository
    {
        public void Dispose()
        {
        }

        public CommittedEventVersion Get(EventProcessorId eventProcessorId)
        {
            return new CommittedEventVersion(0,0,0);
        }

        public void Set(EventProcessorId eventProcessorId, CommittedEventVersion committedEventVersion)
        {
        }
    }

    public class Geodesics : IGeodesics
    {
        public void Dispose()
        {
            
        }

        public ulong GetOffset(EventHorizonKey key)
        {
            return 0;
        }

        public void SetOffset(EventHorizonKey key, ulong offset)
        {
            
        }
    }

    public class EventStore : IEventStore
    {
        public CommittedEventStream Commit(UncommittedEventStream uncommittedEvents)
        {
            return new CommittedEventStream(0, uncommittedEvents.Source, CommitId.New(), uncommittedEvents.CorrelationId, uncommittedEvents.Timestamp, uncommittedEvents.Events);
        }

        public void Dispose()
        {
        }

        public Commits Fetch(EventSourceKey eventSourceKey)
        {
            return new Commits(new CommittedEventStream[0]);
        }

        public Commits FetchFrom(EventSourceKey eventSourceKey, CommitVersion commitVersion)
        {
            return new Commits(new CommittedEventStream[0]);
        }


        public Commits FetchAllCommitsAfter(CommitSequenceNumber commit)
        {
            throw new System.NotImplementedException();
        }

        public SingleEventTypeEventStream FetchAllEventsOfType(ArtifactId eventType)
        {
            throw new System.NotImplementedException();
        }

        public SingleEventTypeEventStream FetchAllEventsOfTypeAfter(ArtifactId eventType, CommitSequenceNumber commit)
        {
            return new SingleEventTypeEventStream(new CommittedEventEnvelope[0]);
        }


        public EventSourceVersion GetCurrentVersionFor(EventSourceKey eventSourceKey)
        {
            return new EventSourceVersion(0,0);
        }

        public EventSourceVersion GetNextVersionFor(EventSourceKey eventSourceKey)
        {
            return new EventSourceVersion(0,0);
        }
    }
}
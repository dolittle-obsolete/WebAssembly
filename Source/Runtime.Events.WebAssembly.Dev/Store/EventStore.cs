// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Artifacts;

namespace Dolittle.Runtime.Events.Store.WebAssembly.Dev
{
    /// <summary>
    /// An InMemory implementation of an <see cref="IEventStore" />
    /// This should never be used as anything other than a testing tool.
    /// </summary>
    public class EventStore : IEventStore
    {
        readonly EventStreamCommitterAndFetcher _eventCommitterAndFetcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStore"/> class.
        /// </summary>
        /// <param name="committerAndFetcher">The <see cref="EventStreamCommitterAndFetcher"/>.</param>
        public EventStore(EventStreamCommitterAndFetcher committerAndFetcher)
        {
            _eventCommitterAndFetcher = committerAndFetcher;
        }

        /// <inheritdoc />
        public CommittedEventStream Commit(UncommittedEventStream uncommittedEvents)
        {
            return _eventCommitterAndFetcher.Commit(uncommittedEvents);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public Commits Fetch(EventSourceKey eventSource)
        {
            return _eventCommitterAndFetcher.Fetch(eventSource);
        }

        /// <inheritdoc />
        public Commits FetchFrom(EventSourceKey eventSource, CommitVersion commitVersion)
        {
            return _eventCommitterAndFetcher.FetchFrom(eventSource, commitVersion);
        }

        /// <inheritdoc />
        public Commits FetchAllCommitsAfter(CommitSequenceNumber commit)
        {
            return _eventCommitterAndFetcher.FetchAllCommitsAfter(commit);
        }

        /// <inheritdoc />
        public SingleEventTypeEventStream FetchAllEventsOfType(ArtifactId eventType)
        {
            return _eventCommitterAndFetcher.FetchAllEventsOfType(eventType);
        }

        /// <inheritdoc />
        public SingleEventTypeEventStream FetchAllEventsOfTypeAfter(ArtifactId eventType, CommitSequenceNumber commit)
        {
            return _eventCommitterAndFetcher.FetchAllEventsOfTypeAfter(eventType, commit);
        }

        /// <inheritdoc />
        public EventSourceVersion GetCurrentVersionFor(EventSourceKey eventSource)
        {
            return _eventCommitterAndFetcher.GetCurrentVersionFor(eventSource);
        }

        /// <inheritdoc />
        public EventSourceVersion GetNextVersionFor(EventSourceKey eventSource)
        {
            return _eventCommitterAndFetcher.GetNextVersionFor(eventSource);
        }
    }
}
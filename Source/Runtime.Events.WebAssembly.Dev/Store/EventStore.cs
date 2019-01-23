/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dolittle.Events;
using Dolittle.Runtime.Events;
using Dolittle.Runtime.Events.Store;
using Dolittle.Artifacts;
using Dolittle.Lifecycle;

namespace Dolittle.Runtime.Events.Store.WebAssembly.Dev
{
    /// <summary>
    /// An InMemory implementation of an <see cref="IEventStore" />
    /// This should never be used as anything other than a testing tool
    /// </summary>
    public class EventStore : IEventStore
    {
        EventStreamCommitterAndFetcher _event_committer_and_fetcher;

        /// <summary>
        /// Instantiates a new instance of the <see cref="EventStore" />
        /// </summary>
        public EventStore(EventStreamCommitterAndFetcher committerAndFetcher)
        {
            _event_committer_and_fetcher = committerAndFetcher;
        }

        /// <inheritdoc />
        public CommittedEventStream Commit(UncommittedEventStream uncommittedEvents)
        {
            ThrowIfDisposed();
            return _event_committer_and_fetcher.Commit(uncommittedEvents);
        }

        void ThrowIfDisposed()
        {
            if(!IsDisposed){
                return;
            }
            throw new ObjectDisposedException("InMemoryEventStore is already disposed");
        }

        /// <summary>
        /// Disposes of the <see cref="EventStore" />
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Indicates whether the <see cref="EventStore" /> has been disposed.
        /// </summary>
        /// <value>true if disposed, false otherwise</value>
        public bool IsDisposed
        {
            get; private set;
        }

        void Dispose(bool disposing)
        {
            IsDisposed = true;
        }
        
        /// <inheritdoc />
        public Commits Fetch(EventSourceKey eventSource)
        {
            ThrowIfDisposed();
            return _event_committer_and_fetcher.Fetch(eventSource);
        }

        /// <inheritdoc />
        public Commits FetchFrom(EventSourceKey eventSource, CommitVersion commitVersion)
        {
            ThrowIfDisposed();
            return _event_committer_and_fetcher.FetchFrom(eventSource,commitVersion);
        }

         /// <inheritdoc />
        public Commits FetchAllCommitsAfter(CommitSequenceNumber commit)
        {
            ThrowIfDisposed();
            return _event_committer_and_fetcher.FetchAllCommitsAfter(commit);
        }
        /// <inheritdoc />
        public SingleEventTypeEventStream FetchAllEventsOfType(ArtifactId eventType) 
        {
            ThrowIfDisposed();
            return _event_committer_and_fetcher.FetchAllEventsOfType(eventType);
        }
        /// <inheritdoc />
        public SingleEventTypeEventStream FetchAllEventsOfTypeAfter(ArtifactId eventType, CommitSequenceNumber commit)
        {
            ThrowIfDisposed();
            return _event_committer_and_fetcher.FetchAllEventsOfTypeAfter(eventType,commit);
        }
        /// <inheritdoc />
        public EventSourceVersion GetCurrentVersionFor(EventSourceKey eventSource)
        {
            ThrowIfDisposed();
            return _event_committer_and_fetcher.GetCurrentVersionFor(eventSource);
        }
        /// <inheritdoc />
        public EventSourceVersion GetNextVersionFor(EventSourceKey eventSource)
        {
            ThrowIfDisposed();
            return _event_committer_and_fetcher.GetNextVersionFor(eventSource);
        }
    }
}
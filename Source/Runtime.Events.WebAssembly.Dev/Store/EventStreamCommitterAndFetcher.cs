// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Dolittle.Artifacts;
using Dolittle.Collections;
using Dolittle.Interaction.WebAssembly.Interop;
using Dolittle.Lifecycle;
using Dolittle.Logging;
using Dolittle.Runtime.Events.WebAssembly.Dev;
using Dolittle.Serialization.Json;
using Dolittle.Types;

namespace Dolittle.Runtime.Events.Store.WebAssembly.Dev
{
    /// <summary>
    /// Manages the committing and fetching of event streams for the <see cref="EventStore" />.
    /// </summary>
    [SingletonPerTenant]
    public class EventStreamCommitterAndFetcher : ICommitEventStreams, IFetchCommittedEvents, IFetchEventSourceVersion
    {
        const string _globalObject = "window._eventStore.eventStore";

        readonly object lockObject = new object();
        readonly List<CommittedEventStream> _commits = new List<CommittedEventStream>();
        readonly HashSet<CommitId> _duplicates = new HashSet<CommitId>();
        readonly ConcurrentDictionary<EventSourceKey, VersionedEventSource> _versions = new ConcurrentDictionary<EventSourceKey, VersionedEventSource>();
        readonly IInstancesOf<IWantToBeNotifiedWhenEventsAreCommited> _commitListeners;
        readonly IJSRuntime _jsRuntime;
        readonly ISerializer _serializer;
        ulong _sequenceNumber = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamCommitterAndFetcher"/> class.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="serializer">JSON <see cref="ISerializer"/>.</param>
        /// <param name="commitListeners">All instances of <see cref="IWantToBeNotifiedWhenEventsAreCommited"/>.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public EventStreamCommitterAndFetcher(IJSRuntime jsRuntime, ISerializer serializer, IInstancesOf<IWantToBeNotifiedWhenEventsAreCommited> commitListeners, ILogger logger)
        {
            _jsRuntime = jsRuntime;
            _serializer = serializer;
            _commitListeners = commitListeners;
            var result = _jsRuntime.Invoke<IEnumerable<CommittedEventStream>>($"{_globalObject}.load").Result;
            logger.Information($"Loaded events {result}");

            _commits.AddRange(result);
            if (_commits.Count > 0)
            {
                _sequenceNumber = _commits.Max(_ => (ulong)_.Sequence);
            }

            logger.Information($"Event Store contains {_commits.Count} events");
        }

        /// <summary>
        /// Increments the count of commits.
        /// </summary>
        /// <returns>The number of commits.</returns>
        public ulong IncrementCount()
        {
            lock (lockObject)
            {
                return ++_sequenceNumber;
            }
        }

        /// <inheritdoc />
        public CommittedEventStream Commit(UncommittedEventStream uncommittedEvents)
        {
            return Commit(uncommittedEvents, IncrementCount());
        }

        /// <inheritdoc />
        public Commits Fetch(EventSourceKey key)
        {
            return new Commits(_commits.Where(c => c.Source.Key == key).ToList());
        }

        /// <inheritdoc />
        public Commits FetchFrom(EventSourceKey key, CommitVersion commitVersion)
        {
            return new Commits(_commits.Where(c => c.Source.Key == key && c.Source.Version.Commit >= commitVersion).ToList());
        }

        /// <inheritdoc />
        public Commits FetchAllCommitsAfter(CommitSequenceNumber commit)
        {
            return new Commits(_commits.Where(c => c.Sequence > commit).ToList());
        }

        /// <inheritdoc />
        public SingleEventTypeEventStream FetchAllEventsOfType(ArtifactId artifactId)
        {
            var commits = _commits.Where(c => c.Events.Any(e => e.Metadata.Artifact.Id == artifactId));
            return GetEventsFromCommits(commits, artifactId);
        }

        /// <inheritdoc />
        public SingleEventTypeEventStream FetchAllEventsOfTypeAfter(ArtifactId artifactId, CommitSequenceNumber commitSequenceNumber)
        {
            var commits = _commits.Where(c => c.Sequence > commitSequenceNumber && c.Events.Any(e => e.Metadata.Artifact.Id == artifactId));
            return GetEventsFromCommits(commits, artifactId);
        }

        /// <inheritdoc />
        public EventSourceVersion GetCurrentVersionFor(EventSourceKey eventSource)
        {
            if (_versions.TryGetValue(eventSource, out VersionedEventSource v))
            {
                return v.Version;
            }

            return EventSourceVersion.NoVersion;
        }

        /// <inheritdoc />
        public EventSourceVersion GetNextVersionFor(EventSourceKey eventSource)
        {
            return GetCurrentVersionFor(eventSource).NextCommit();
        }

        CommittedEventStream Commit(UncommittedEventStream uncommittedEvents, CommitSequenceNumber commitSequenceNumber)
        {
            lock (lockObject)
            {
                ThrowIfDuplicate(uncommittedEvents.Id);
                ThrowIfConcurrencyConflict(uncommittedEvents.Source);

                var commit = new CommittedEventStream(commitSequenceNumber, uncommittedEvents.Source, uncommittedEvents.Id, uncommittedEvents.CorrelationId, uncommittedEvents.Timestamp, uncommittedEvents.Events);
                _commits.Add(commit);
                _duplicates.Add(commit.Id);
                _versions.AddOrUpdate(commit.Source.Key, commit.Source, (id, ver) => commit.Source);

                var commitsAsJson = _serializer.ToJson(_commits, SerializationOptions.CamelCase);
                _jsRuntime.Invoke($"{_globalObject}.save", commitsAsJson);

                _commitListeners.ForEach(_ => _.Handle(commit));

                return commit;
            }
        }

        void ThrowIfDuplicate(CommitId commitId)
        {
            if (!_duplicates.Contains(commitId))
                return;

            throw new CommitIsADuplicate();
        }

        void ThrowIfConcurrencyConflict(VersionedEventSource version)
        {
            if (_versions.TryGetValue(version.Key, out VersionedEventSource ver))
            {
                if (ver == version || ver.Version.Commit >= version.Version.Commit)
                {
                    throw new EventSourceConcurrencyConflict();
                }
            }
        }

        SingleEventTypeEventStream GetEventsFromCommits(IEnumerable<CommittedEventStream> commits, ArtifactId eventType)
        {
            var events = new List<CommittedEventEnvelope>();
            foreach (var commit in commits)
            {
                events.AddRange(commit.Events.FilteredByEventType(eventType).Select(e => new CommittedEventEnvelope(commit.Sequence, e.Metadata, e.Event)));
            }

            return new SingleEventTypeEventStream(events);
        }
    }
}
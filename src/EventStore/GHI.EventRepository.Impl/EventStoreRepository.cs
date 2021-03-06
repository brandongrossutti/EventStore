﻿using System;
using System.Collections.Generic;
using System.Linq;
using EventStore;
using GHI.Domain;
using GHI.EventRepository.Impl.SnapShotting;
using GHI.EventRepository.Impl.UnitOfWork;

namespace GHI.EventRepository.Impl
{
    public class EventStoreRepository : IRepository<Guid>, IDisposable
    {
        private IStoreEvents _eventStore;
        private readonly Dictionary<Guid, AggregateRoot> _cachedRoots;
        private readonly SnapShotTracker _snapShotTracker;

        public EventStoreRepository(IStoreEvents eventStore, ISnapShotStrategy snapShotStrategy)
        {
            _eventStore = eventStore;
            _cachedRoots = new Dictionary<Guid, AggregateRoot>();
            _snapShotTracker = new SnapShotTracker(snapShotStrategy);
        }

        public T GetAggregateRoot<T>(Guid id) where T : AggregateRoot, new()
        {
            AggregateRoot root = null;
            _cachedRoots.TryGetValue(id, out root);
            if (root == null)
            {
                root = LoadAggregateRootFromStream<T>(id);
                AddCachedRoot(root);
            }
            T ret = (T) root;
            EventStoreUnitOfWork.RegisterAggregateRoot<Guid>(ret);
            return ret;
        }

        private AggregateRoot LoadAggregateRootFromStream<T>(Guid id) where T :  AggregateRoot, new()
        {
            IEventStream stream;
            Snapshot snapShot = _eventStore.Advanced.GetSnapshot(id, int.MaxValue);
            if (snapShot != null)
            {
                return LoadFromSnapShot<T>(id, snapShot);
            }
            stream = _eventStore.OpenStream(id, 0, int.MaxValue);
            List<IEvent> nonSnapShotEvents = stream.CommittedEvents.Select(committedEvent => committedEvent.Body as IEvent).ToList();
            T nonSnapShotRoot = new T();
            nonSnapShotRoot.LoadFromRepository(nonSnapShotEvents);
            return nonSnapShotRoot;
        }

        private T LoadFromSnapShot<T>(Guid id, Snapshot snapShot) where T : AggregateRoot, new()
        {
            T root = (T)snapShot.Payload;
            int lastSnapShotSequence = snapShot.StreamRevision;
            _snapShotTracker.SetLastSequence(id, lastSnapShotSequence);
            IEventStream stream = _eventStore.OpenStream(snapShot, int.MaxValue);
            List<IEvent> events = stream.CommittedEvents.Select(committedEvent => committedEvent.Body as IEvent).ToList();
            root.LoadFromRepository(events);
            return root;
        }

        private void TakeSnapshot(AggregateRoot root)
        {
            IEventStream stream = _eventStore.OpenStream(root.Id, 0, int.MaxValue);
            Snapshot snapshot = new Snapshot(root.Id, stream.CommitSequence, root);
            _eventStore.Advanced.AddSnapshot(snapshot);
            _snapShotTracker.SetLastSequence(root.Id, stream.CommitSequence);
        }

        public void Save<TY>(TY root) where TY : AggregateRoot, new()
        {
            AddCachedRoot(root);
            EventStoreUnitOfWork.RegisterAggregateRoot<Guid>(root);
        }

        private IEventStream OpenStream(Guid id)
        {
            return _eventStore.OpenStream(id, 0, int.MaxValue);
        }

        public void Dispose()
        {
            _eventStore = null;
            GC.SuppressFinalize(this);
        }

        private void AddCachedRoot(AggregateRoot aggregateRoot)
        {
            if (!_cachedRoots.ContainsKey(aggregateRoot.Id))
            {
                _cachedRoots.Add(aggregateRoot.Id, aggregateRoot);
            }
        }

        public void Commit(AggregateRoot aggregateRoot)
        {
            if (aggregateRoot.HasUncommittedEvents)
            {
                Guid commitId = Guid.NewGuid();
                IEventStream eventStream = OpenStream(aggregateRoot.Id);
                foreach (IEvent uncommittedEvent in aggregateRoot.UncommittedEvents)
                {
                    EventMessage message = new EventMessage();
                    message.Body = uncommittedEvent;
                    eventStream.Add(message);
                }
                eventStream.CommitChanges(commitId);

                if(_snapShotTracker.ShouldSnapShot(aggregateRoot.Id, eventStream.CommitSequence))
                {
                    TakeSnapshot(aggregateRoot);
                }
                aggregateRoot.ClearUncommitedEvents();
            }
        }
    }
}

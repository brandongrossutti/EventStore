using System;
using System.Collections.Generic;
using System.Linq;
using EventStore;
using GHI.EventRepository.Impl.UnitOfWork;

namespace GHI.EventRepository.Impl
{
    public class EventStoreRepository : IRepository<Guid>, IDisposable
    {
        private IStoreEvents _eventStore;
        private readonly Dictionary<Guid, AggregateRoot> _cachedRoots;

        public EventStoreRepository(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
            _cachedRoots = new Dictionary<Guid, AggregateRoot>();
        }

        public T GetAggregateRoot<T>(Guid id) where T : AggregateRoot, new()
        {
            AggregateRoot root;
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
            IEventStream stream = LoadRoot(id);
            List<IEvent> events =
                stream.CommittedEvents.Select(committedEvent => committedEvent.Body as IEvent).ToList();
            T root = new T();
            root.LoadFromRepository(events);
            return root;
        }

        private void TakeSnapshot(AggregateRoot root)
        {
            IEventStream stream = _eventStore.OpenStream(root.Id, 0, int.MaxValue);
            Snapshot snapshot = new Snapshot(root.Id, stream.CommitSequence, root);
            _eventStore.Advanced.AddSnapshot(snapshot);
        }

        private IEventStream LoadRoot(Guid id)
        {
            IEventStream stream;
            Snapshot snapShot = _eventStore.Advanced.GetSnapshot(id, int.MaxValue);
            if (snapShot != null)
            {
                stream = _eventStore.OpenStream(snapShot, int.MaxValue);
                return stream;
            }
            stream = _eventStore.OpenStream(id, 0, int.MaxValue);
            return stream;
        }

        public void Save<TY>(TY root) where TY : AggregateRoot, new()
        {
            AddCachedRoot(root);
            EventStoreUnitOfWork.RegisterAggregateRoot<Guid>(root);
        }

        public IEventStream OpenStream(Guid id)
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
    }
}

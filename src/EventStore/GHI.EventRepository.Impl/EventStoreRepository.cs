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
        private readonly Dictionary<Guid, object> _trackedRoots;

        public EventStoreRepository(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
            _trackedRoots = new Dictionary<Guid, object>();
        }

        public T GetAggregateRoot<T>(Guid id) where T : AggregateRoot, new()
        {
            object root;
            _trackedRoots.TryGetValue(id, out root);
            if (root == null)
            {
                IEventStream stream = _eventStore.OpenStream(id, 0, int.MaxValue);
                List<IEvent> events =
                    stream.CommittedEvents.Select(committedEvent => committedEvent.Body as IEvent).ToList();
                T obj = new T();
                obj.LoadFromRepository(events);
                _trackedRoots.Add(id,obj);
                EventStoreUnitOfWork.RegisterAggregateRoot<Guid>(obj);
                return obj;
            }
            T ret = (T) root;
            EventStoreUnitOfWork.RegisterAggregateRoot<Guid>(ret);
            return ret;
        }

        public void Save<TY>(TY root) where TY : AggregateRoot, new()
        {
            AddTrackedRoot(root);
            EventStoreUnitOfWork.RegisterAggregateRoot<Guid>(root);
        }

        public IEventStream OpenStream(Guid id)
        {
            return _eventStore.OpenStream(id, 0, int.MaxValue);
        }

        public void Dispose()
        {
            _eventStore = null;
        }

        public void AddTrackedRoot(AggregateRoot aggregateRoot)
        {
            if (!_trackedRoots.ContainsKey(aggregateRoot.Id))
            {
                _trackedRoots.Add(aggregateRoot.Id, aggregateRoot);
            }
        }
    }
}

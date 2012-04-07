using System;
using System.Collections.Generic;
using System.Linq;
using EventStore;

namespace GHI.EventRepository.Impl
{
    public class EventStoreRepository : IRepository<Guid>, IDisposable
    {
        private IStoreEvents _eventStore;
        private Dictionary<Guid, object> _trackedRoots;

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
                return obj;
            }
            return (T) root;
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
            _trackedRoots.Add(aggregateRoot.Id, aggregateRoot);   
        }
    }
}

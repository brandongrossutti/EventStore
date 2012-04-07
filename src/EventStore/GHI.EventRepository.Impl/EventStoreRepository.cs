using System;
using System.Collections.Generic;
using System.Linq;
using EventStore;

namespace GHI.EventRepository.Impl
{
    public class EventStoreRepository : IRepository<Guid>, IDisposable
    {
        private readonly IStoreEvents _eventStore;

        public EventStoreRepository(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public T GetAggregateRoot<T>(Guid id) where T : AggregateRoot, new()
        {
            IEventStream stream = _eventStore.OpenStream(id, 0, int.MaxValue);
            List<IEvent> events = stream.CommittedEvents.Select(committedEvent => committedEvent.Body as IEvent).ToList();
            T obj = new T();
            obj.LoadFromRepository(events);
            return obj;
        }

        public IStoreEvents EventStore
        {
            get { return _eventStore; }
        }

        public IEventStream OpenStream(Guid id)
        {
            return _eventStore.OpenStream(id, 0, int.MaxValue);
        }

        public void Dispose()
        {
            //todo
        }
    }
}

using System;
using System.Collections.Generic;
using EventStore;

namespace GHI.EventRepository.Impl
{
    public class EventStoreRepository : IRepository<Guid>
    {
        private IStoreEvents _eventStore;

        public EventStoreRepository(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public T GetAggregateRoot<T>(Guid id) where T : AggregateRoot, new()
        {
            IEventStream stream = _eventStore.OpenStream(Guid.NewGuid(), 0, int.MaxValue);
            List<IEvent> events = new List<IEvent>();
            foreach (EventMessage committedEvent in stream.CommittedEvents)
            {

            }

            T obj = new T();
            obj.LoadFromRepository(events);
            return obj;
        }

        public IEventStream OpenStream(Guid id)
        {
            return _eventStore.OpenStream(Guid.NewGuid(), 0, int.MaxValue);
        }
    }
}

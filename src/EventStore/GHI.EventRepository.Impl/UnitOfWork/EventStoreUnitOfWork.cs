using System.Collections.Generic;
using EventStore;
using GHI.Commons.UnitOfWork;

namespace GHI.EventRepository.Impl.UnitOfWork 
{
    public class EventStoreUnitOfWork : IUnitOfWork
    {
        private readonly EventStoreRepository _eventStoreRepository;
        private readonly List<AggregateRoot> _aggregateRootsAffected;

        public EventStoreUnitOfWork(EventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
            _aggregateRootsAffected = new List<AggregateRoot>();
        }

        public void Commit()
        {
            foreach (AggregateRoot aggregateRoot in _aggregateRootsAffected)
            {
                IEventStream eventStream = _eventStoreRepository.OpenStream(aggregateRoot.Id);
                foreach (IEvent uncommittedEvent in aggregateRoot.UncommittedEvents)
                {
                    EventMessage message = new EventMessage();
                    message.Body = uncommittedEvent;
                    eventStream.Add(message);
                }
                eventStream.CommitChanges(aggregateRoot.Id);    
            }
        }

        public void RollBack()
        {
            foreach (AggregateRoot aggregateRoot in _aggregateRootsAffected)
            {
                aggregateRoot.ClearUncommitedEvents();
                aggregateRoot.ReloadAggregateRoot();
            }
        }

        public void Dispose()
        {
            _aggregateRootsAffected.Clear();
        }

        public void RegisterAggregateRoot(AggregateRoot root)
        {
            _aggregateRootsAffected.Add(root);
        }
    }
}
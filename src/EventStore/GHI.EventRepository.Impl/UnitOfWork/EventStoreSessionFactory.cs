using EventStore;
using GHI.Commons.UnitOfWork;

namespace GHI.EventRepository.Impl.UnitOfWork
{
    public class EventStoreSessionFactory : ISessionFactory
    {
        private readonly EventStoreRepository _eventStore;

        public EventStoreSessionFactory(EventStoreRepository eventStore)
        {
            _eventStore = eventStore;
        }

        public IUnitOfWork GetCurrentUnitOfWork()
        {
            return new EventStoreUnitOfWork(_eventStore);
        }
    }
}

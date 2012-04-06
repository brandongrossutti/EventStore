using GHI.Commons.UnitOfWork;

namespace GHI.EventRepository.Impl.UnitOfWork
{
    public class EventStoreUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly EventStoreRepository _repository;

        public EventStoreUnitOfWorkFactory(EventStoreRepository repository)
        {
            _repository = repository;
        }

        public IUnitOfWork Create()
        {
            return new EventStoreUnitOfWork(_repository);
        }
    }
}
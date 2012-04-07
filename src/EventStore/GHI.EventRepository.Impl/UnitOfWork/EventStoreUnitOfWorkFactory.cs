using System;
using GHI.Commons.UnitOfWork;

namespace GHI.EventRepository.Impl.UnitOfWork
{
    public class EventStoreUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IRepository<Guid> _repository;

        public EventStoreUnitOfWorkFactory(IRepository<Guid> repository)
        {
            _repository = repository;
        }

        public IUnitOfWork Create()
        {
            return new EventStoreUnitOfWork(_repository);
        }
    }
}
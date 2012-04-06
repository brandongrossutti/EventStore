using System;
using GHI.Commons.Context;
using GHI.Commons.UnitOfWork;

namespace GHI.EventRepository.Impl.UnitOfWork
{
    public class EventStoreUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly EventStoreRepository _eventStoreRepository;

        public EventStoreUnitOfWorkFactory(EventStoreRepository _eventStoreRepository)
        {
            _eventStoreRepository = _eventStoreRepository;
        }

        public IUnitOfWork Create()
        {
            ILocalContext context = new ThreadLocalContext();
            context.Set(LocalContextKey.EVENT_STORE_KEY, CurrentSession());
            return new EventStoreUnitOfWork(_eventStoreRepository);
        }

        public ISession CurrentSession()
        {
            throw new NotImplementedException();
        }

        public ISessionFactory SessionFactory
        {
            get { throw new NotImplementedException(); }
        }

    }
}

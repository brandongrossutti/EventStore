using System;
using GHI.Bus;
using GHI.Commons.UnitOfWork;
using GHI.EventRepository;
using GHI.EventRepository.Impl.UnitOfWork;

namespace GHI.EventStore.Tests
{
    public class CreateNewTestAggregateRootCommandHandler : IMessageHandler<CreateNewTestAggregateRootCommand>
    {
        private readonly ISessionFactory  _sessionFactory;
        private readonly IRepository<Guid> _repository;

        public CreateNewTestAggregateRootCommandHandler(ISessionFactory sessionFactory, IRepository<Guid> repository)
        {
            _sessionFactory = sessionFactory;
            _repository = repository;
        }

        public void HandleMessage(CreateNewTestAggregateRootCommand message)
        {
            TestAggregateRoot root = new TestAggregateRoot(message.Id);

            EventStoreUnitOfWork currentUnitOfWork = _sessionFactory.GetCurrentUnitOfWork() as EventStoreUnitOfWork;
            if (currentUnitOfWork == null)
            {
                throw new ArgumentException("Incorrect Unit of Work, event store implementation must be eventstoreUnitofWork");
            }
            currentUnitOfWork.RegisterAggregateRoot(root);
        }
    }
}
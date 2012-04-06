using System;
using GHI.Bus;
using GHI.Commons.UnitOfWork;
using GHI.EventRepository;
using GHI.EventRepository.Impl.UnitOfWork;

namespace GHI.EventStore.Tests
{
    public class CreateNewTestAggregateRootCommandHandler : IMessageHandler<CreateNewTestAggregateRootCommand>
    {
        private readonly IRepository<Guid> _repository;

        public CreateNewTestAggregateRootCommandHandler(IRepository<Guid> repository)
        {
            _repository = repository;
        }

        public void HandleMessage(CreateNewTestAggregateRootCommand message)
        {
            TestAggregateRoot root = new TestAggregateRoot(message.Id);
            EventStoreUnitOfWork.RegisterAggregateRoot<Guid>(root);
            //EventStoreUnitOfWork currentUnitOfWork = _sessionFactory.GetCurrentUnitOfWork() as EventStoreUnitOfWork;
            //if (currentUnitOfWork == null)
            //{
            //    throw new ArgumentException("Incorrect Unit of Work, event store implementation must be eventstoreUnitofWork");
            //}
            //currentUnitOfWork.RegisterAggregateRoot(root);
        }
    }
}   
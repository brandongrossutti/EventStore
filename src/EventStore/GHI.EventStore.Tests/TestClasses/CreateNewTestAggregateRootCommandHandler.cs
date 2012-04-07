using System;
using GHI.Bus;
using GHI.EventRepository;
using GHI.EventRepository.Impl.UnitOfWork;

namespace GHI.EventStore.Tests.TestClasses
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
        }
    }
}   
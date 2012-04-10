using System;
using GHI.Bus;
using GHI.EventRepository;
using GHI.TestDomain.Messages;
using GHI.TestDomain.Model;

namespace GHI.TestDomain.Handlers
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
            _repository.Save(root);
        }
    }
}   
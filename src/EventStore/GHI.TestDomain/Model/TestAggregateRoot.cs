using System;
using GHI.Domain;
using GHI.Domain.Mapping;
using GHI.TestDomain.Events;
using GHI.TestDomain.Messages;

namespace GHI.TestDomain.Model
{
    public class TestAggregateRoot : AggregateRoot
    {
        private Guid _id;

        public string Address { get; private set; }
        
        public TestAggregateRoot(Guid id)
        {
            OnEvent(new CreateTestAggregateRootEvent(id));
        }

        public void TestAggregateRoute(CreateTestAggregateRootCommand command)
        {
            OnEvent(new CreateTestAggregateRootEvent(command.AggregateId));
        }

        public TestAggregateRoot() { }

        public void OnCreateTestAggregateRoot(CreateTestAggregateRootEvent @event)
        {
            _id = @event.Id;
        }

        public void ExecuteChangeAddressCommand(ChangeAddressCommand command)
        {
            OnEvent(new AddressChangedEvent(command.Value));
        }

        public void ChangeAddress(string value)
        {
            OnEvent(new AddressChangedEvent(value));
        }

        public void OnAddressChanged(AddressChangedEvent @event)
        {
            Address = @event.Value;
        }

        public override Guid Id
        {
            get { return _id; }
        }
    }

    public class CreateTestAggregateRootCommand:Command
    {
        public CreateTestAggregateRootCommand(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
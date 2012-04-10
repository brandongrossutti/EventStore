using System;
using GHI.EventRepository;
using GHI.TestDomain.Events;

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

        public TestAggregateRoot() { }

        public void OnCreateTestAggregateRoot(CreateTestAggregateRootEvent @event)
        {
            _id = @event.Id;
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
}
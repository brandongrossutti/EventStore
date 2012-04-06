using GHI.EventRepository;

namespace GHI.EventStore.Tests
{
    public class AddressChangedEvent : IEvent
    {
        public AddressChangedEvent(string value)
        {
            Value = value;
        }

        public string Value{get;private set;}
    }
}
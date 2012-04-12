using GHI.Domain;
using GHI.EventRepository;

namespace GHI.TestDomain.Events
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
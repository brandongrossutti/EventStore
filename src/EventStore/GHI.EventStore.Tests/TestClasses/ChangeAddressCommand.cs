using System;
using GHI.Bus;

namespace GHI.EventStore.Tests.TestClasses
{
    public class ChangeAddressCommand : Message
    {
        public ChangeAddressCommand(Guid id, string value)
        {
            Id = id;
            Value = value;
        }

        public Guid Id { get; set; }
        public string Value{get;private set;}
    }
}

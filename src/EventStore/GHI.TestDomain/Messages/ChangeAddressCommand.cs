using System;
using GHI.Bus;
using GHI.Domain.Mapping;

namespace GHI.TestDomain.Messages
{
    [Serializable]
    public class ChangeAddressCommand : Command
    {
        public ChangeAddressCommand(Guid id, string value):base(id)
        {
            Id = id;
            Value = value;
        }



        public Guid Id { get; set; }
        public string Value{get;private set;}
    }
}

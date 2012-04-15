using System;
using GHI.Bus;
using GHI.Domain.Mapping;

namespace GHI.TestDomain.Messages
{
    [Serializable]
    public class CreateNewTestAggregateRootCommand : Command
    {
        private Guid _id;
        public CreateNewTestAggregateRootCommand(Guid id):base(id)
        {
            _id = id;
        }

        public Guid Id
        {
            get { return _id; }
        }
    }
}

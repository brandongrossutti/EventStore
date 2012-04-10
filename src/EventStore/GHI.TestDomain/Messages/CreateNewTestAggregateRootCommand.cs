using System;
using GHI.Bus;

namespace GHI.TestDomain.Messages
{
    [Serializable]
    public class CreateNewTestAggregateRootCommand : Message
    {
        private Guid _id;
        public CreateNewTestAggregateRootCommand(Guid id)
        {
            _id = id;
        }

        public Guid Id
        {
            get { return _id; }
        }
    }
}

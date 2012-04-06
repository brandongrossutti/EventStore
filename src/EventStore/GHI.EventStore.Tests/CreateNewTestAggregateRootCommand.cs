using System;
using GHI.Bus;

namespace GHI.EventStore.Tests
{
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

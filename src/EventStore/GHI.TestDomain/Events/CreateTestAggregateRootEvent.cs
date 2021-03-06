using System;
using GHI.Domain;

namespace GHI.TestDomain.Events
{
    public class CreateTestAggregateRootEvent : IEvent
    {
        private readonly Guid _id;

        public CreateTestAggregateRootEvent(Guid id)
        {
            _id = id;
        }

        public Guid Id
        {
            get { return _id; }
        }
    }
}
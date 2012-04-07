using System;
using GHI.EventRepository;

namespace GHI.EventStore.Tests.TestClasses
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
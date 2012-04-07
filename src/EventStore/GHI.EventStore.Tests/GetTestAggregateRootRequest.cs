using System;
using GHI.Bus;

namespace GHI.EventStore.Tests
{
    public class GetTestAggregateRootRequest : IRequest<GetTestAggregateRootResponse>
    {
        private readonly Guid _id;

        public GetTestAggregateRootRequest(Guid id)
        {
            _id = id;
        }

        public Guid Id
        {
            get { return _id; }
        }
    }
}
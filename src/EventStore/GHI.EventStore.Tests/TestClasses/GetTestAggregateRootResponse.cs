using System;
using GHI.Bus;

namespace GHI.EventStore.Tests.TestClasses
{
    public class GetTestAggregateRootResponse : Response
    {
        private readonly Guid _id;

        public GetTestAggregateRootResponse(Guid id)
        {
            _id = id;
            Success = true;
        }

        public GetTestAggregateRootResponse()
        {
            Success = false;
        }

        public Guid Id
        {
            get { return _id; }
        }
    }
}
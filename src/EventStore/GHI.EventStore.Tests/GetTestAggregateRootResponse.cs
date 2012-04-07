using System;
using GHI.Bus;

namespace GHI.EventStore.Tests
{
    public class GetTestAggregateRootResponse : Response
    {
        private Guid _id;

        public GetTestAggregateRootResponse(Guid id)
        {
            _id = id;
        }

        public GetTestAggregateRootResponse()
        {
            throw new NotImplementedException();
        }

        public Guid Id
        {
            get {
                return _id;
            }
        }
    }
}
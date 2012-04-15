using System;
using GHI.Bus;
using GHI.Domain.Mapping;

namespace GHI.TestDomain.Messages
{
    [Serializable]
    public class GetTestAggregateRootRequest : Command<GetTestAggregateRootResponse>
    {
        private readonly Guid _id;

        public GetTestAggregateRootRequest(Guid id):base(id)
        {
            _id = id;
        }

        public Guid Id
        {
            get { return _id; }
        }
    }
}
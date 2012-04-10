using System;
using GHI.Bus;
using GHI.EventRepository;
using GHI.TestDomain.Messages;
using GHI.TestDomain.Model;

namespace GHI.TestDomain.Handlers
{
    public class GetTestAggregateRootRequestHandler : IRequestHandler<GetTestAggregateRootRequest, GetTestAggregateRootResponse>
    {
        private readonly IRepository<Guid> _repository;

        public GetTestAggregateRootRequestHandler(IRepository<Guid> repository)
        {
            _repository = repository;
        }

        public GetTestAggregateRootResponse HandleRequest(GetTestAggregateRootRequest request)
        {
            TestAggregateRoot root = _repository.GetAggregateRoot<TestAggregateRoot>(request.Id);
            return new GetTestAggregateRootResponse(root.Id);
        }
    }
}

using System;
using GHI.Bus;
using GHI.Domain.Mapping;
using GHI.EventRepository;
using GHI.TestDomain.Messages;
using GHI.TestDomain.Model;

namespace GHI.TestDomain.Handlers
{
    public class GetTestAggregateRootCommandHandler : IRequestHandler<GetTestAggregateRootRequest, GetTestAggregateRootResponse>
    {
        private readonly IRepository<Guid> _repository;

        public GetTestAggregateRootCommandHandler(IRepository<Guid> repository)
        {
            _repository = repository;
        }

        public GetTestAggregateRootResponse HandleCommand(GetTestAggregateRootRequest request)
        {
            TestAggregateRoot root = _repository.GetAggregateRoot<TestAggregateRoot>(request.Id);
            return new GetTestAggregateRootResponse(root.Id);
        }
    }
}

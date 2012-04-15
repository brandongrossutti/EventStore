using System;
using GHI.Commons.UnitOfWork;
using GHI.Domain.Mapping;
using GHI.EventRepository;
using GHI.TestDomain.Messages;
using GHI.TestDomain.Model;
using GHI.WireUp;
using NUnit.Framework;
using StructureMap;
using IContainer = GHI.Commons.IOC.IContainer;

namespace GHI.DomainMapping.Tests
{
    [TestFixture]
    public class TestMapping
    {
        [Test]
        public void TestAggregateRootMapping()
        {
            InitializerWireUp wireUp = new InitializerWireUpBuilder()
                .LoadAssemblyPrefix("GHI")
                .ShouldRunDefault(true)
                .WithAssemblyNotReferencedToLoad("GHI.EventRepository.Impl")
                .WithAssemblyNotReferencedToLoad("GHI.Bus.InMemory");

            IContainer container = (IContainer)ObjectFactory.GetInstance(typeof(IContainer));
            IRepository<Guid> repository = container.GetInstance<IRepository<Guid>>();
            IUnitOfWorkFactory unitOfWorkFactory = container.GetInstance<IUnitOfWorkFactory>();

            AggregateRootInspector inspector = new AggregateRootInspector(unitOfWorkFactory, container, repository);
            inspector.InspectAggregateRoot(typeof(TestAggregateRoot));

            Guid aggregateId = Guid.NewGuid();
            inspector.ExecuteHandler(new CreateNewTestAggregateRootCommand(aggregateId));
            inspector.ExecuteHandler(new ChangeAddressCommand(aggregateId, "testAddress"));

        }
    }
}

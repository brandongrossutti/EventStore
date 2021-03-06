﻿using System;
using GHI.Bus;
using GHI.Domain.Mapping;
using GHI.TestDomain.Messages;
using GHI.TestDomain.Model;
using GHI.WireUp;
using NUnit.Framework;
using StructureMap;
using IContainer = GHI.Commons.IOC.IContainer;

namespace GHI.EventRepository.Tests
{
    [TestFixture]
    public class TestWiringOfBus
    {
        [Test]
        public void TestMessageSend()
        {
            InitializerWireUp wireUp = new InitializerWireUpBuilder()
                .LoadAssemblyPrefix("GHI")
                .ShouldRunDefault(true)
                .WithAssemblyNotReferencedToLoad("GHI.EventRepository.Impl")
                .WithAssemblyNotReferencedToLoad("GHI.Bus.InMemory");

            IContainer container = (IContainer) ObjectFactory.GetInstance(typeof(IContainer));
            AggregateRootInspector inspector = (AggregateRootInspector) container.GetInstance<IHandlerResolver>();
            inspector.InspectAggregateRoot(typeof(TestAggregateRoot));
            IMessagePublisher publisher = container.GetInstance<IMessagePublisher>();
            Guid id = Guid.NewGuid();
            publisher.SendMessage(new CreateNewTestAggregateRootCommand(id));

            IRequestResponseClient requestResponseClient = container.GetInstance<IRequestResponseClient>();
            GetTestAggregateRootResponse response = (GetTestAggregateRootResponse) requestResponseClient.SendRequest<GetTestAggregateRootResponse>(new GetTestAggregateRootRequest(id));

            publisher.SendMessage(new ChangeAddressCommand(id, "test"));
            publisher.SendMessage(new ChangeAddressCommand(id, "test"));
            publisher.SendMessage(new ChangeAddressCommand(id, "SnapshotTime"));
            publisher.SendMessage(new ChangeAddressCommand(id, "test"));
            publisher.SendMessage(new ChangeAddressCommand(id, "test"));

            Assert.AreEqual(id,response.Id);
        }
    }
}

        
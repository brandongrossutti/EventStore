using System;
using GHI.Bus;
using GHI.EventRepository.Impl;
using GHI.WireUp;
using NUnit.Framework;
using StructureMap;
using IContainer = GHI.Commons.IOC.IContainer;

namespace GHI.EventStore.Tests
{
    [TestFixture]
    public class TestWiringOfBus
    {
        [Test]
        public void TestMessageSend()
        {
            InitializerWireUp wireup = new InitializerWireUp("GHI", true);
            wireup.AddInitialization(Initializer.GetWireUp());
            wireup.AddInitialization(GHI.Bus.InMemory.Initializer.GetWireUp(wireup));
            wireup.Initialize();

            IContainer container = (IContainer) ObjectFactory.GetInstance(typeof(IContainer));
            IMessagePublisher publisher = container.GetInstance<IMessagePublisher>();
            Guid id = Guid.NewGuid();
            publisher.SendMessage(new CreateNewTestAggregateRootCommand(id));

        }
    }
}

        
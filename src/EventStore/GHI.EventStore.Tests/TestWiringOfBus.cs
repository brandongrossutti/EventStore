using System;
using System.Collections.Generic;
using System.Reflection;
using GHI.Bus;
using GHI.EventStore.Tests.TestClasses;
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
            var assembliesNotReferencedToLoad = new List<AssemblyName>()
                                                    {
                                                        new AssemblyName("GHI.EventRepository.Impl"), 
                                                        new AssemblyName("GHI.Bus.InMemory")
                                                    };
            InitializerWireUp wireup = new InitializerWireUp("GHI", true, assembliesNotReferencedToLoad);

            IContainer container = (IContainer) ObjectFactory.GetInstance(typeof(IContainer));
            IMessagePublisher publisher = container.GetInstance<IMessagePublisher>();
            Guid id = Guid.NewGuid();
            publisher.SendMessage(new CreateNewTestAggregateRootCommand(id));

            IRequestResponseClient requestResponseClient = container.GetInstance<IRequestResponseClient>();
            GetTestAggregateRootResponse response = (GetTestAggregateRootResponse) requestResponseClient.SendRequest(new GetTestAggregateRootRequest(id));

            Assert.AreEqual(id,response.Id);
        }
    }
}

        
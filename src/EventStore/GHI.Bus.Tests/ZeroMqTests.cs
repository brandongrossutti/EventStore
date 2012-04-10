using System;
using System.Threading;
using GHI.TestDomain.Messages;
using GHI.WireUp;
using NUnit.Framework;
using StructureMap;
using IContainer = GHI.Commons.IOC.IContainer;

namespace GHI.Bus.Tests
{
    [TestFixture]
    public class ZeroMqTests
    {
        [Test]
        public void TestSend()
        {
            InitializerWireUp wireUp = new InitializerWireUpBuilder()
                .LoadAssemblyPrefix("GHI")
                .WithAssemblyNotReferencedToLoad("GHI.EventRepository.Impl")
                .WithAssemblyNotReferencedToLoad("GHI.Bus.ZeroMQ")
                .ShouldRunDefault(true);

            IContainer container = (IContainer)ObjectFactory.GetInstance(typeof(IContainer));
            IMessagePublisher publisher = container.GetInstance<IMessagePublisher>();

            IMessageSubscriber subscriber = container.GetInstance<IMessageSubscriber>();

            Guid id = Guid.NewGuid();
            Thread.Sleep(2000);
            publisher.SendMessage(new CreateNewTestAggregateRootCommand(id));
            publisher.SendMessage(new ChangeAddressCommand(id, "test"));
            publisher.SendMessage(new ChangeAddressCommand(id, "test"));
            publisher.SendMessage(new ChangeAddressCommand(id, "SnapshotTime"));
            publisher.SendMessage(new ChangeAddressCommand(id, "test"));
            publisher.SendMessage(new ChangeAddressCommand(id, "test"));

            Thread.Sleep(5000);
            subscriber.Dispose();
            publisher.Dispose();
        }
    }
    
    [Serializable]
    public class TestMessage : Message
    {
        public string Test { get; set; }

        public TestMessage(string test)
        {
            Test = test;
        }
    }
}

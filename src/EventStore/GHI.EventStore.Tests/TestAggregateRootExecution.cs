using System;
using NUnit.Framework;

namespace GHI.EventStore.Tests
{
    [TestFixture]
    public class TestAggregateRootExecution
    {
        [Test]
        public void TestEventExecutionOfAggregateRoot()
        {
            TestAggregateRoot aggregateRootImpl = new TestAggregateRoot(Guid.NewGuid());
            aggregateRootImpl.ChangeAddress("testingSuccess");
            Assert.AreEqual(aggregateRootImpl.Address, "testingSuccess");
            
        }
    }
}

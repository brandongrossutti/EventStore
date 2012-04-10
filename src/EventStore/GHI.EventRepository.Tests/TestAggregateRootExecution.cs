using System;
using GHI.TestDomain.Model;
using NUnit.Framework;

namespace GHI.EventRepository.Tests
{
    [TestFixture]
    public class TestAggregateRootExecution
    {
        [Test]
        public void TestEventExecutionOfAggregateRoot()
        {
            TestAggregateRoot aggregateRootImpl = new TestAggregateRoot(Guid.NewGuid());
            aggregateRootImpl.ChangeAddress("testingSuccess");
            Assert.AreEqual("testingSuccess",aggregateRootImpl.Address);
        }
    }
}

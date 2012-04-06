using System;
using GHI.Commons.UnitOfWork;

namespace GHI.EventStore.Tests
{
    public class NullSession : ISession
    {
        public IUnitOfWork GetCurrentUnitOfWork()
        {
            throw new NotImplementedException();
        }
    }
}
using System;

namespace GHI.Commons.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void RollBack();
        
    }
}

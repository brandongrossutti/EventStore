using GHI.Commons.UnitOfWork;

namespace GHI.EventRepository
{
    public interface IRepository<T>
    {
        TY GetAggregateRoot<TY>(T id) where TY : AggregateRoot, new();
    }
}
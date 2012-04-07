namespace GHI.EventRepository
{
    public interface IRepository<T>
    {
        TY GetAggregateRoot<TY>(T id) where TY : AggregateRoot, new();
        void Save<TY>(TY root) where TY : AggregateRoot, new();
    }
}
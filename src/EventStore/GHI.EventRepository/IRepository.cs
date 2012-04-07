namespace GHI.EventRepository
{
    public interface IRepository<T>
    {
        TY GetAggregateRoot<TY>(T id) where TY : AggregateRoot, new();
        /// <summary>
        /// New items must be implicitly saved, 
        /// </summary>
        /// <typeparam name="TY"></typeparam>
        /// <param name="root"></param>
        void Save<TY>(TY root) where TY : AggregateRoot, new();
    }
}
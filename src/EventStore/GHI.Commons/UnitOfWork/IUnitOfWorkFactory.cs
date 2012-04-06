namespace GHI.Commons.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
        ISession CurrentSession();
        ISessionFactory SessionFactory { get; }
    }
}
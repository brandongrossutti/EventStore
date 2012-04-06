namespace GHI.Commons.UnitOfWork
{
    public interface ISessionFactory
    {
        IUnitOfWork GetCurrentUnitOfWork();
    }
}
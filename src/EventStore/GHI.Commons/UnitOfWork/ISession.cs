namespace GHI.Commons.UnitOfWork
{
    public interface ISession
    {
        IUnitOfWork GetCurrentUnitOfWork();
    }
}
namespace GHI.Bus
{
    public interface IRequestResponseServer
    {
        Response ProcessRequest<T>(Command<T> command) where T : Response;
    }
}

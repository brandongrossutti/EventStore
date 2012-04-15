namespace GHI.Bus
{
    public interface IRequestResponseClient
    {
        Response SendRequest<T>(Command<T> request) where T : Response, new();
    }
}

namespace GHI.Bus
{
    public interface IRequestResponseClient
    {
        Response SendRequest<T>(IRequest<T> request) where T : Response, new();
    }
}

namespace GHI.Bus
{
    public interface IRequestResponseClient
    {
        T SendRequest<T>(Message request) where T : Response, new();
    }
}

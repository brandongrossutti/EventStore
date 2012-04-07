namespace GHI.Bus
{
    public interface IRequestHandler<in TRequest, out TResponse> where TRequest : IRequest where TResponse : Response, new()
    {
        TResponse HandleRequest(TRequest request);
    }
}

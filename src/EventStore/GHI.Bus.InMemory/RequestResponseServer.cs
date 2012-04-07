namespace GHI.Bus.InMemory
{
    public class RequestResponseServer : IRequestResponseServer
    {
        private readonly HandlerResolver _resolver;

        public RequestResponseServer(HandlerResolver resolver)
        {
            _resolver = resolver;
        }

        public Response ProcessRequest(IRequest request)
        {
            return _resolver.ExecuteRequestHandler(request);
        }
    }
}
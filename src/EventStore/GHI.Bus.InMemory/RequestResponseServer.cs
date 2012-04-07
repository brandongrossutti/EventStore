namespace GHI.Bus.InMemory
{
    public class RequestResponseServer : IRequestResponseServer
    {
        private readonly IHandlerResolver _resolver;

        public RequestResponseServer(IHandlerResolver resolver)
        {
            _resolver = resolver;
        }

        public Response ProcessRequest(IRequest request)
        {
            return _resolver.ExecuteRequestHandler(request);
        }
    }
}
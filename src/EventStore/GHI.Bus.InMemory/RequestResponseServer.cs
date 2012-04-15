using GHI.Domain.Mapping;

namespace GHI.Bus.InMemory
{
    public class RequestResponseServer : IRequestResponseServer
    {
        private readonly IHandlerResolver _resolver;

        public RequestResponseServer(IHandlerResolver resolver)
        {
            _resolver = resolver;
        }

        public Response ProcessRequest<T>(Command<T> command) where T: Response
        {
            return _resolver.ExecuteRequestHandler<T>(command);
        }
    }
}
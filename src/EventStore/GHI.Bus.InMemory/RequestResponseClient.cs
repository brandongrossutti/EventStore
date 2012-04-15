using GHI.Domain.Mapping;

namespace GHI.Bus.InMemory
{
    public class RequestResponseClient : IRequestResponseClient
    {
        private readonly IRequestResponseServer _server;

        public RequestResponseClient(IRequestResponseServer server)
        {
            _server = server;
        }

        public Response SendRequest<T>(Command<T> request) where T : Response, new()
        {
            return _server.ProcessRequest<T>(request);
        }
    }
}
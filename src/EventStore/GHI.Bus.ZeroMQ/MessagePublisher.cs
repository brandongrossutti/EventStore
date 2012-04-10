using System;
using ZMQ;

namespace GHI.Bus.ZeroMQ
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly OnTheWireBusConfiguration _configuration;
        private readonly Socket _publisher;
        private Context _context;

        public MessagePublisher(OnTheWireBusConfiguration configuration)
        {
            _configuration = configuration;
            _context = new Context(configuration.MaxThreads);
            _publisher = _context.Socket(SocketType.PUB);
            _publisher.Bind(configuration.FullyQualifiedAddress);
        }


        public void SendMessage(Message message)
        {
            SendStatus status = _publisher.Send(_configuration.Serialize(message));
        }

        public void Dispose()
        {
            _publisher.Dispose();
        }
    }
}

namespace GHI.Bus.InMemory
{
    public class MessageSubscriber : IMessageSubscriber
    {
        private readonly IHandlerResolver _resolver;

        public MessageSubscriber(IHandlerResolver resolver)
        {
            _resolver = resolver;
        }

        public void ReceiveMessage<T>(T message) where T : Message
        {
           _resolver.ExecuteHandler(message);
        }
    }
}




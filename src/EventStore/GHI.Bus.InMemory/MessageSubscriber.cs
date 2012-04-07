namespace GHI.Bus.InMemory
{
    public class MessageSubscriber : IMessageSubscriber
    {
        private readonly HandlerResolver _resolver;

        public MessageSubscriber(HandlerResolver resolver)
        {
            _resolver = resolver;
        }

        public void ReceiveMessage<T>(T message) where T : Message
        {
           _resolver.ExecuteHandler(message);
        }
    }
}




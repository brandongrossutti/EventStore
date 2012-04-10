namespace GHI.Bus.InMemory
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IMessageSubscriber _messageSubscriber;
        public MessagePublisher(IMessageSubscriber messageSubscriber)
        {
            _messageSubscriber = messageSubscriber;
        }

        public void SendMessage(Message message)
        {
            _messageSubscriber.ReceiveMessage(message);
        }

        public void Dispose()
        {
            
        }
    }
}
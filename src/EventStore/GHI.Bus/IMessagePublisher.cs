namespace GHI.Bus
{
    public interface IMessagePublisher
    {
        void SendMessage(Message message);
    }
}
namespace GHI.Bus
{
    public interface IMessageHandler<in T> where T : Message
    {
        void HandleMessage(T message);
    }
}
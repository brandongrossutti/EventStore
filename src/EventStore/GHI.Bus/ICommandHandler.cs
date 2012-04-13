namespace GHI.Bus
{
    public interface ICommandHandler<in T> where T : Message
    {
        void HandleCommand(T message);
    }
}
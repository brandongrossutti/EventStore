using GHI.Bus;

namespace GHI.Domain.Mapping
{
    public interface ICommandHandler<in T> where T : Command
    {
        void HandleCommand(T message);
    }
}
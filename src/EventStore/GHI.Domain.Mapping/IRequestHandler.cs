using GHI.Bus;

namespace GHI.Domain.Mapping
{
    public interface IRequestHandler<in TCommand, out TResponse> where TCommand : Command where TResponse : Response, new()
    {
        TResponse HandleCommand(TCommand request);
    }
}

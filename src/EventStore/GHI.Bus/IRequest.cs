namespace GHI.Bus
{
    public interface IRequest {}
    public interface IRequest<T>: IRequest where T : Response, new() {}
}
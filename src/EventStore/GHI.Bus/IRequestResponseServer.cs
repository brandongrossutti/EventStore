namespace GHI.Bus
{
    public interface IRequestResponseServer
    {
        Response ProcessRequest(IRequest request);
    }
}

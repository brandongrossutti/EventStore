namespace GHI.WireUp
{
    public interface IInitializer
    {
        WireUpItem GetWireUp(InitializerWireUp wireup);
    }
}
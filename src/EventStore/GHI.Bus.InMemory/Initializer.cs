using System.Reflection;
using GHI.WireUp;

namespace GHI.Bus.InMemory
{
    public static class Initializer
    {
        public static WireUpItem GetWireUp(InitializerWireUp wireup)
        {
            return new WireUpItem(
                x => x.Scan(
                    s =>
                        {
                            foreach (Assembly assembly in wireup.Assemblies)
                            {
                                s.Assembly(assembly);
                                s.Convention<MessageHandlerTypeConvention>();
                                s.Convention<RequestHandlerTypeConvention>();
                            }
                            s.WithDefaultConventions();
                        }));
        }
    }
}

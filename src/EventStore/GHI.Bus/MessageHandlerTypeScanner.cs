using System;
using StructureMap.Graph;

namespace GHI.Bus
{
    public class MessageHandlerTypeScanner : ITypeScanner
    {
        public void Process(Type type, PluginGraph graph)
        {
            Type[] interfaces = type.GetInterfaces();
            foreach (Type interfaceType in interfaces)
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IMessageHandler<>))
                {
                    graph.Configure(x => x.ForRequestedType(interfaceType).TheDefaultIsConcreteType(type));
                }
            }
        }
    }
}

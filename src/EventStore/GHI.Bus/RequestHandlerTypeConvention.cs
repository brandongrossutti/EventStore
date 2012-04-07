using System;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace GHI.Bus
{
    public class RequestHandlerTypeConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            Type[] interfaces = type.GetInterfaces();
            foreach (Type interfaceType in interfaces)
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                {
                    registry.For(interfaceType).Use(type);
                }
            }
        }
    }
}

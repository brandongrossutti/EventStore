using System.Reflection;
using GHI.Commons.Configuration;
using GHI.Commons.IOC;
using GHI.WireUp;
using StructureMap.Attributes;

namespace GHI.Bus.InMemory
{
    public  class Initializer
    {
        public static WireUpItem GetWireUp(InitializerWireUp wireup)
        {
            return new WireUpItem(
                x =>
                    {
                        x.ForRequestedType<IConfigurationProvider>()
                            .TheDefaultIsConcreteType<InMemoryConfigurationProvider>()
                            .CacheBy(InstanceScope.Singleton);

                        x.ForRequestedType<IContainer>()
                            .TheDefaultIsConcreteType<StructureMapContainer>()
                            .CacheBy(InstanceScope.Singleton);

                        x.ForRequestedType<IMessageSubscriber>()
                            .TheDefaultIsConcreteType<MessageSubscriber>()
                            .CacheBy(InstanceScope.Singleton);

                        x.ForRequestedType<IMessagePublisher>()
                            .TheDefaultIsConcreteType<MessagePublisher>()
                            .CacheBy(InstanceScope.Singleton);

                        x.Scan(
                            s =>
                                {
                                    foreach (Assembly assembly in wireup.Assemblies)
                                    {
                                        s.Assembly(assembly);
                                        s.With<MessageHandlerTypeScanner>();
                                    }
                                    s.WithDefaultConventions();
                                });


                    });
        }
    }
}

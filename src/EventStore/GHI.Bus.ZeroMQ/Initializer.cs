using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using GHI.WireUp;
using StructureMap;

namespace GHI.Bus.ZeroMQ
{
    public class Initializer: IInitializer
    {
        public WireUpItem GetWireUp(InitializerWireUp wireup)
        {
            OnTheWireBusConfiguration configuration = new OnTheWireBusConfiguration(10, @"tcp://127.0.0.1:5565",
                                                                                    new BinaryFormatter(), new ASCIIEncoding() );
            ObjectFactory.Inject<OnTheWireBusConfiguration>(configuration);
            return new WireUpItem(
                x =>
                    {
                        x.For<IMessageSubscriber>()
                            .Singleton()
                            .Use<MessageSubscriber>();


                        x.For<IMessagePublisher>()
                            .Singleton()
                            .Use<MessagePublisher>();


                        x.For<IHandlerResolver>()
                            .Use<HandlerResolver>();


                        x.Scan(
                            s =>
                                {
                                    foreach (Assembly assembly in wireup.Assemblies)
                                    {
                                        s.Assembly(assembly);
                                        s.Convention<MessageHandlerTypeConvention>();
                                        s.Convention<RequestHandlerTypeConvention>();
                                    }
                                    //s.WithDefaultConventions();
                                }
                            );
                    }
                );
        }
    }
}

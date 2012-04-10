using System;
using System.Text;
using System.Threading;
using ZMQ;

namespace GHI.Bus.ZeroMQ
{
    public class MessageSubscriber : IMessageSubscriber
    {
        private readonly IHandlerResolver _resolver;
        private Thread _subscriberThread;
        private static bool _runLoop = true;
        private static object _lockObject;

        public MessageSubscriber(OnTheWireBusConfiguration configuration, IHandlerResolver resolver)
        {
            _resolver = resolver;
            _lockObject = new object();
            _subscriberThread = new Thread(RecieveMessages);
            _subscriberThread.Start(new object[] {configuration, resolver, new Action<Message>(ProcessMessage)});
        }

        private static void RecieveMessages(object o)
        {
            object[] obj = o as object[];
            OnTheWireBusConfiguration configuration = (OnTheWireBusConfiguration) obj[0];
            IHandlerResolver resolver = (IHandlerResolver) obj[1];
            Action<Message> handlerDelegate = (Action<Message>) obj[2];
            using (var context = new Context(configuration.MaxThreads))
            {
                using (Socket subscriber = context.Socket(SocketType.SUB))
                {
                    subscriber.Subscribe("", Encoding.Unicode);
                    subscriber.Connect(configuration.FullyQualifiedAddress);

                    while (_runLoop && subscriber.Backlog!=0)
                    {
                        lock (_lockObject)
                        {
                            byte[] buffer = subscriber.Recv(1);
                            if (buffer != null)
                            {
                                Message message = (Message) configuration.Deserialize(buffer);
                                handlerDelegate(message);
                            }
                        }
                    }
                    Console.WriteLine("exiting loop");
                }
            }
        }

        public void ProcessMessage(Message message)
        {
            _resolver.ExecuteHandler(message);
        }

        public void ReceiveMessage<T>(T message) where T : Message
        {
            _resolver.ExecuteHandler(message);
        }

        public void Dispose()
        {
            lock (_lockObject)
            {
                _runLoop = false;
                _subscriberThread = null;
            }
        }
    }
}

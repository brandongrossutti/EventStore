using System;
using System.Collections.Generic;

namespace GHI.Bus.InMemory
{
    public class MessageSubscriber : IMessageSubscriber
    {
        private readonly HandlerResolver _resolver;
        private readonly Dictionary<Type, List<Action<Message>>> _routes = new Dictionary<Type, List<Action<Message>>>();

        public MessageSubscriber(HandlerResolver resolver)
        {
            _resolver = resolver;
        }

        public void ReceiveMessage<T>(T message) where T : Message
        {
           _resolver.ExecuteHandler(message);
        }
    }
}




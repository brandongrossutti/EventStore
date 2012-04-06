using System;

namespace GHI.Bus
{
    public interface IMessageSubscriber
    {
        void ReceiveMessage<T>(T message) where T : Message;
    }
}
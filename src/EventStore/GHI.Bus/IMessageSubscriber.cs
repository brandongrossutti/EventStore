using System;

namespace GHI.Bus
{
    public interface IMessageSubscriber : IDisposable
    {
        void ReceiveMessage<T>(T message) where T : Message;
    }
}
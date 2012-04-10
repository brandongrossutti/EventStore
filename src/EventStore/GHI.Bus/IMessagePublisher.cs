using System;

namespace GHI.Bus
{
    public interface IMessagePublisher : IDisposable
    {
        void SendMessage(Message message);
    }
}
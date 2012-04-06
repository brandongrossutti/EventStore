using System;

namespace GHI.Bus
{
    public interface IRequestResponseServer
    {
        void ProcessRequest(Message request);
    }
}

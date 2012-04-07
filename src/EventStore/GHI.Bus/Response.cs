using System;

namespace GHI.Bus
{
    public class Response : Message
    {
        public bool Success;

        public void Failed(string message, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
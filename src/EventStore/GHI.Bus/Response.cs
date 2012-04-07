using System;

namespace GHI.Bus
{
    public class Response : Message
    {
        public bool Success;
        private string _failureMessage;

        public string FailureMessage
        {
            get { return _failureMessage; }
        }

        public void Failed(string message, Exception exception)
        {
            _failureMessage = String.Format("Failed Request {0} with exception {1}", message, exception.ToString());
        }
    }
}
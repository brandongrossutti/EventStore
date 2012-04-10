using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace GHI.Bus
{
    public class OnTheWireBusConfiguration
    {
        private readonly int _maxThreads;
        private readonly string _fullyQualifiedAddress;
        private readonly BinaryFormatter _serializationFormatter;
        private readonly Encoding _encoding;

        public OnTheWireBusConfiguration(int maxThreads, string fullyQualifiedAddress, BinaryFormatter serializationFormatter, Encoding encoding)
        {
            _maxThreads = maxThreads;
            _fullyQualifiedAddress = fullyQualifiedAddress;
            _serializationFormatter = serializationFormatter;
            _encoding = encoding;
        }

        public int MaxThreads
        {
            get {
                return _maxThreads;
            }
        }

        public string FullyQualifiedAddress
        {
            get {
                return _fullyQualifiedAddress;
            }
        }

        public byte[] Serialize(object obj)//, string topic)
        {
            MemoryStream stream = new MemoryStream();
            //stream.Write(_encoding.GetBytes(topic + '\x00'),0,topic.Length + 1);
            _serializationFormatter.Serialize(stream, obj);
            stream.Close();
            return stream.ToArray();
        }

        public object Deserialize(byte[] msg)//, string topic)
        {
            MemoryStream stream = new MemoryStream(msg);
            //stream.Seek(topic.Length + 1, SeekOrigin.Begin);
            object obj = _serializationFormatter.Deserialize(stream);
            stream.Close();
            return obj;
        }
    }
}
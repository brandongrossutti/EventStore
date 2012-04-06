using System;
using System.Collections.Generic;

namespace GHI.Commons.Context
{
    public class ThreadLocalContext : ILocalContext
    {
        [ThreadStatic]
        private static IDictionary<string, object> Dictionary;
        private static readonly object _lockObject = new object();

        public object Get(string key)
        {
            return GetDictionary()[key];
        }

        public void Set(string key, object value)
        {
            GetDictionary()[key] = value;
        }

        private IDictionary<string, object> GetDictionary()
        {
            lock (_lockObject)
            {
                if (Dictionary == null)
                {
                    Dictionary = new Dictionary<string, object>();
                }
                return Dictionary;
            }
        }
    }
}
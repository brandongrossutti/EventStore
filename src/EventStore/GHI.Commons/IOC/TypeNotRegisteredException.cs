using System;

namespace GHI.Commons.IOC
{
    public class TypeNotRegisteredException : Exception
    {
        private readonly Type _type;

        public TypeNotRegisteredException(Type type, Exception exception)
            : base(string.Format("The type '{0}' is not registered in the container.", type.FullName), exception)
        {
            _type = type;
        }

        public Type Type
        {
            get { return _type; }
        }
    }
}
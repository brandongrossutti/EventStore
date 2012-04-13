using System;
using System.Collections;
using System.Collections.Generic;
using StructureMap;
using StructureMap.Query;

namespace GHI.Commons.IOC
{
    public class StructureMapContainer : IContainer
    {
        private readonly object lockObject = new object();

        public object GetInstance(Type serviceType)
        {
            lock (lockObject)
            {
                try
                {
                    return ObjectFactory.GetInstance(serviceType);
                }
                catch (StructureMapException exception)
                {
                    if (exception.Message.StartsWith("StructureMap Exception Code:  202"))
                    {
                        throw new TypeNotRegisteredException(serviceType, exception);
                    }

                    throw;
                }
            }
        }

        public object TryGetInstance(Type serviceType)
        {
            lock (lockObject)
            {
                {
                    return ObjectFactory.TryGetInstance(serviceType);
                }
            }
        }


        public object GetInstance(Type serviceType, string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return (IEnumerable<object>)ObjectFactory.GetAllInstances(serviceType);
        }

        public T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T));
        }

        public T GetInstance<T>(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllInstances<T>()
        {
            return ObjectFactory.GetAllInstances<T>();
        }

        public void DisposeAllInstances()
        {
            foreach (IPluginTypeConfiguration pluginType in ObjectFactory.Model.PluginTypes)
            {
                IList instances = ObjectFactory.GetAllInstances(pluginType.PluginType);
                foreach (object instance in instances)
                {
                    if (instance is IDisposable)
                    {
                        ((IDisposable)instance).Dispose();
                    }
                }
            }
        }


    }
}
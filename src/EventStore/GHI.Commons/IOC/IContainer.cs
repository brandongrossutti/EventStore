using System;
using System.Collections.Generic;

namespace GHI.Commons.IOC
{
    public interface IContainer
    {
        object GetInstance(Type serviceType);
        object GetInstance(Type serviceType, string key);
        IEnumerable<object> GetAllInstances(Type serviceType);
        T GetInstance<T>();
        T GetInstance<T>(string key);
        IEnumerable<T> GetAllInstances<T>();
        void DisposeAllInstances();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GHI.Commons.Extensions;
using GHI.Commons.IOC;
using StructureMap;
using StructureMap.Attributes;
using log4net;
using IContainer = GHI.Commons.IOC.IContainer;

namespace GHI.WireUp
{
    public class InitializerWireUp
    {
        private readonly List<WireUpItem> _wireUpItems;
        private ILog _log;
        readonly List<Assembly> _assemblies;
        

        public InitializerWireUp(string assemblyPrefix, bool runDefaults)
        {
            _wireUpItems = new List<WireUpItem>();
            _log = LogManager.GetLogger(GetType());
            Assembly.GetAssembly(GetType()).LoadAllDependencies(x => x.Name.StartsWith(assemblyPrefix));
            _assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.StartsWith(assemblyPrefix)).ToList();
            if (runDefaults) RunDefaults();
        }

        private void RunDefaults()
        {
            _wireUpItems.Add(new WireUpItem(
                                 x =>
                                     {
                                         x.ForRequestedType<IContainer>()
                                             .TheDefaultIsConcreteType<StructureMapContainer>()
                                             .CacheBy(InstanceScope.Singleton);

                                         x.Scan(
                                             s =>
                                                 {
                                                     foreach (Assembly assembly in _assemblies)
                                                     {
                                                         s.Assembly(assembly);
                                                     }
                                                     s.WithDefaultConventions();
                                                 });
                                     }));
        }




        public List<Assembly> Assemblies
        {
            get { return _assemblies; }
           
        }


        public void Initialize()
        {

            foreach (WireUpItem item in _wireUpItems)
            {   
                ObjectFactory.Configure(item.Expression);
            }
            ObjectFactory.AssertConfigurationIsValid();
        }

        public void AddInitialization(WireUpItem item)
        {
            _wireUpItems.Add(item);
        }
    }
}

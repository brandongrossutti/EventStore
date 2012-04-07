using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GHI.Commons.Extensions;
using GHI.Commons.IOC;
using StructureMap;
using log4net;
using IContainer = GHI.Commons.IOC.IContainer;

namespace GHI.WireUp
{
    public class InitializerWireUp
    {
        private readonly List<WireUpItem> _wireUpItems;
        private ILog _log;
        readonly List<Assembly> _assemblies;
        

        public InitializerWireUp(string assemblyPrefix, bool runDefaults, IEnumerable<AssemblyName> assembliesNotReferencedToLoad)
        {
            _wireUpItems = new List<WireUpItem>();
            _log = LogManager.GetLogger(GetType());
            foreach (var assemblyName in assembliesNotReferencedToLoad)
            {
                AppDomain.CurrentDomain.Load(assemblyName);
            }
            
            Assembly.GetAssembly(Assembly.GetExecutingAssembly().GetType()).LoadAllDependencies(x => x.Name.StartsWith(assemblyPrefix));
            _assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.StartsWith(assemblyPrefix)).ToList();
            if (runDefaults) RunDefaults();


            foreach (var assembly in _assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (Type @interface in type.GetInterfaces())
                    {
                        if (@interface.FullName == typeof(IInitializer).FullName)
                        {
                            IInitializer initializer = (IInitializer) Activator.CreateInstance(type);
                            AddInitialization(initializer.GetWireUp(this));
                        }
                    }
                }
            }

            Initialize();
        }

        private void RunDefaults()
        {
            _wireUpItems.Add(new WireUpItem(
                                 x =>
                                     {
                                         x.For<IContainer>()
                                             .Singleton()
                                             .Use<StructureMapContainer>();


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



        public IEnumerable<Assembly> Assemblies
        {
            get { return _assemblies; }
        }


        private void Initialize()
        {
            foreach (WireUpItem item in _wireUpItems)
            {   
                ObjectFactory.Configure(item.Expression);
            }
            ObjectFactory.AssertConfigurationIsValid();
        }

        private void AddInitialization(WireUpItem item)
        {
            _wireUpItems.Add(item);
        }
    }
}

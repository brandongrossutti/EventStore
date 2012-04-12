using System;
using System.Collections.Generic;
using System.Reflection;
using GHI.Commons.UnitOfWork;
using GHI.EventRepository;

namespace GHI.Domain.Mapping
{
    public class AggregateRootInspector
    {
        private readonly Dictionary<Type, MethodInfo> _aggregateHandlers;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public AggregateRootInspector(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _aggregateHandlers = new Dictionary<Type,MethodInfo>();
        }

        public void InspectDomainAssembly(string assemblyName)
        {
            Assembly assembly = AppDomain.CurrentDomain.Load(assemblyName);
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof (AggregateRoot)))
                {
                    InspectAggregateRoot(type);
                }
            }
        }

        public void InspectAggregateRoot(Type root) 
        {
            foreach (MethodInfo publicMethod in root.GetMethods())
            {
                foreach (ParameterInfo parameter in publicMethod.GetParameters())
                {
                    if(parameter.ParameterType.IsSubclassOf(typeof(Command)))
                    {
                        _aggregateHandlers.Add(parameter.ParameterType, publicMethod);
                    }
                }
            }
        }

        public void RouteCommand(IRepository<Guid> repository, Command command) 
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    MethodInfo commandHandler;
                    _aggregateHandlers.TryGetValue(command.GetType(), out commandHandler);

                    if (commandHandler == null)
                    {
                        //log that not registered
                        return;
                    }
                    else
                    {
                        Type aggregateType = commandHandler.ReflectedType;
                        AggregateRoot root = (AggregateRoot)typeof(IRepository<Guid>)
                                                                 .GetMethod("GetAggregateRoot")
                                                                 .MakeGenericMethod(aggregateType)
                                                                 .Invoke(repository, new object[] { command.AggregateId });

                        if (root != null)
                        {
                            commandHandler.Invoke(root, new object[] { command });
                        }
                    }
                    unitOfWork.Commit();

                }
                catch (Exception)
                {
                    //log
                    unitOfWork.RollBack();
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using GHI.Bus;
using GHI.Commons.IOC;
using GHI.Commons.UnitOfWork;
using GHI.EventRepository;

namespace GHI.Domain.Mapping
{
    public class AggregateRootInspector
    {
        private readonly Dictionary<Type, MethodInfo> _aggregateHandlers;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IContainer _container;

        public AggregateRootInspector(IUnitOfWorkFactory unitOfWorkFactory, IContainer container)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _container = container;
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
                    Type commandHandlerType = typeof(ICommandHandler<>).MakeGenericType(new[] { command.GetType() });
                    object commandHandler = commandHandler = _container.TryGetInstance(commandHandlerType);
                    if (commandHandler != null)
                    {
                        RouteToOverride(commandHandler, command);
                    }
                    else
                    {
                        MethodInfo commandHandlerMethod;
                        _aggregateHandlers.TryGetValue(command.GetType(), out commandHandlerMethod);

                        if (commandHandlerMethod == null)
                        {
                            //log that not registered
                            return;
                        }
                        else
                        {
                            Type aggregateType = commandHandlerMethod.ReflectedType;
                            AggregateRoot root = (AggregateRoot) typeof (IRepository<Guid>)
                                                                     .GetMethod("GetAggregateRoot")
                                                                     .MakeGenericMethod(aggregateType)
                                                                     .Invoke(repository,
                                                                             new object[] {command.AggregateId});

                            if (root != null)
                            {
                                commandHandlerMethod.Invoke(root, new object[] {command});
                            }
                            else
                            {
                                //log throw
                            }
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

        private void RouteToOverride(object commandHandler, Command command)
        {
            commandHandler.GetType().GetMethod("HandleCommand").Invoke(commandHandler, new[] { command });
        }
    }
}
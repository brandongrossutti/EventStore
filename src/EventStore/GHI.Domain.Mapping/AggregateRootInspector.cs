using System;
using System.Collections.Generic;
using System.Reflection;
using GHI.Bus;
using GHI.Commons.IOC;
using GHI.Commons.UnitOfWork;
using GHI.EventRepository;

namespace GHI.Domain.Mapping
{
    public class AggregateRootInspector : IHandlerResolver
    {
        private readonly Dictionary<Type, MethodInfo> _aggregateHandlers;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IContainer _container;
        private readonly IRepository<Guid> _repository;

        public AggregateRootInspector(IUnitOfWorkFactory unitOfWorkFactory, IContainer container, IRepository<Guid> repository)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _container = container;
            _repository = repository;
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

        private Response RouteCommand<T>(Command command) 
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    Type responseType = typeof (T);
                    Response response=null;
                    Type commandHandlerType = typeof(ICommandHandler<>).MakeGenericType(new[] { command.GetType() });

                    object commandHandler =  _container.TryGetInstance(commandHandlerType);

                    //request response
                    if (commandHandler == null)
                    {
                        Type[] paramaters =  { command.GetType(), responseType };
                        commandHandlerType = typeof(IRequestHandler<,>).MakeGenericType(paramaters);
                        commandHandler = _container.TryGetInstance(commandHandlerType);
                    }
                    if (commandHandler != null)
                    {
                        response = RouteToOverride<T>(commandHandler, command);
                    }
                    else
                    {
                        MethodInfo commandHandlerMethod;
                        _aggregateHandlers.TryGetValue(command.GetType(), out commandHandlerMethod);

                        if (commandHandlerMethod == null)
                        {
                            //log that not registered
                            response = (Response)Activator.CreateInstance(responseType);
                            response.Failed("not registered", new Exception("could not find handler"));
                            return response;
                        }
                        Type aggregateType = commandHandlerMethod.ReflectedType;
                        AggregateRoot root = (AggregateRoot) typeof (IRepository<Guid>)
                                                                 .GetMethod("GetAggregateRoot")
                                                                 .MakeGenericMethod(aggregateType)
                                                                 .Invoke(_repository,
                                                                         new object[] {command.AggregateId});

                        if (root != null)
                        {
                             response = commandHandlerMethod.Invoke(root, new object[] {command}) as Response;
                        }
                        else
                        {
                            //log throw
                        }
                    }
                    unitOfWork.Commit();
                    return response;


                }
                catch (Exception exception)
                {
                    //log
                    unitOfWork.RollBack();

                }
            }
            Response defaultResponse = new Response();
            defaultResponse.Success = false;
            return defaultResponse;
        }

        private Response RouteToOverride<T>(object commandHandler, Command command) // where T: Response
        {
            Console.WriteLine("Executed overrideHandler: " + command.ToString());
            return commandHandler.GetType().GetMethod("HandleCommand").Invoke(commandHandler, new[] { command }) as Response;
        }

        public void ExecuteHandler(Message message)
        {
            RouteCommand<Response>((Command)message);
        }

        public Response ExecuteRequestHandler<T>(Command request)
        {
            return RouteCommand<T>(request);
        }
    }

    public interface IHandlerResolver
    {
        void ExecuteHandler(Message message);
        Response ExecuteRequestHandler<T>(Command request);
    }
}
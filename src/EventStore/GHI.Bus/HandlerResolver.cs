using System;
using System.Reflection;
using GHI.Commons.IOC;
using GHI.Commons.Logging;
using GHI.Commons.UnitOfWork;


namespace GHI.Bus
{
    public class HandlerResolver : IHandlerResolver
    {
        private readonly IContainer _container;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ILogEvents _logger;

        public HandlerResolver(IContainer container, IUnitOfWorkFactory unitOfWorkFactory, ILogEvents logger)
        {
            _container = container;
            _unitOfWorkFactory = unitOfWorkFactory;
            _logger = logger;
        }

        public void ExecuteHandler(Message message)
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    Type messageHandlerType = typeof(IMessageHandler<>).MakeGenericType(new[] { message.GetType() });
                    object messageHandler;
                    try
                    {
                        messageHandler = _container.GetInstance(messageHandlerType);
                    }
                    catch (TypeNotRegisteredException)
                    {
                        _logger.Log(String.Format("There is no handler registered for the message type '{0}'.",messageHandlerType.FullName),LogLevel.Error,GetType());
                        return;
                    }
                    messageHandler.GetType().GetMethod("HandleMessage").Invoke(messageHandler, new[] { message });
                    unitOfWork.Commit();
                    _logger.Log("commited", LogLevel.Info, GetType());
                }
                catch (Exception exception)
                {
                    _logger.Log(exception.ToString(), LogLevel.Error, GetType());
                    unitOfWork.RollBack();
                }
            }
        }

        public Response ExecuteRequestHandler(IRequest request)
        {
            Response response;

            Type requestType = request.GetType();
            Type responseType = GetResponseType(requestType);

            Type requestHandlerType = typeof(IRequestHandler<,>).MakeGenericType(new[] { requestType, responseType });
            object requestHandler;
            try
            {
                requestHandler = _container.GetInstance(requestHandlerType);
            }
            catch (TypeNotRegisteredException)
            {
                string errorMessage = string.Format("There is no handler registered for the request type '{0}'.",
                                                    requestType.FullName);
                _logger.Log(errorMessage, LogLevel.Error, GetType());
                response = (Response)Activator.CreateInstance(responseType);
                response.Failed(errorMessage, null);
                return response;
            }

            MethodInfo handleRequestMethodInfo = requestHandler.GetType().GetMethod("HandleRequest");

            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    response = (Response)handleRequestMethodInfo.Invoke(requestHandler, new[] { request });
                    if (response.Success)
                    {
                        unitOfWork.Commit();
                    }
                    else
                    {
                        unitOfWork.RollBack();
                    }
                }
                catch (Exception exception)
                {
                    response = (Response)Activator.CreateInstance(responseType);
                    string message = exception.InnerException != null ? exception.InnerException.Message : exception.Message;
                    response.Failed(message, exception);
                    unitOfWork.RollBack();
                    _logger.Log(string.Format("Unable to process request {0}{1}{2}", request, Environment.NewLine, exception), LogLevel.Error, GetType());
                }

                return response;
            }
        }

        private Type GetResponseType(Type requestType)
        {
            Type[] requestInterfaceTypes = requestType.GetInterfaces();
            Type responseType = null;
            foreach (Type requestInterfaceType in requestInterfaceTypes)
            {
                if (requestInterfaceType.Name == "IRequest`1")
                {
                    responseType = requestInterfaceType.GetGenericArguments()[0];
                    break;
                }
            }
            return responseType;
        }
    }

    public interface IHandlerResolver
    {
        void ExecuteHandler(Message message);
        Response ExecuteRequestHandler(IRequest request);
    }
}

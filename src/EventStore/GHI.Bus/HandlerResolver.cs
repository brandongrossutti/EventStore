using System;
using GHI.Commons.IOC;
using GHI.Commons.UnitOfWork;
using log4net;


namespace GHI.Bus
{
    public class HandlerResolver
    {
        private readonly IContainer _container;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ILog _log;

        public HandlerResolver(IContainer container, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _container = container;
            _unitOfWorkFactory = unitOfWorkFactory;
            _log = LogManager.GetLogger(GetType());
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
                        _log.DebugFormat("There is no handler registered for the message type '{0}'.",
                                         messageHandlerType.FullName);
                        return;
                    }
                    messageHandler.GetType().GetMethod("HandleMessage").Invoke(messageHandler, new[] { message });
                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.RollBack();
                }
            }
        }
    }
}

using RUCore.Common.Clients;
using RUCore.Common.Handlers;

namespace RUCore.Common.Invoking
{
    /// <summary>
    /// Message subscription.
    /// </summary>
    public interface IMessageSubscription : IMessageHandler, IDisposable, IEnumerable<IMessageHandler>
    {
        /// <summary>
        /// Add a handler to the subscription.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        DynamicHandlerRegistration AddHandler(IMessageHandler handler);
    }

    /// <summary>
    /// Message subscription.
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMessageSubscription<TClient, TMessage> : IMessageSubscription,
                                                               IMessageHandler<TClient, TMessage>
        where TClient : IMessageClient
        where TMessage : IMessage
    {
        /// <summary>
        /// Add a handler to the subscription.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        DynamicHandlerRegistration AddHandler(IMessageHandler<TClient, TMessage> handler);

        Task IMessageHandler.HandleMessageAsync(IMessageClient client, IMessage message)
        {
            return HandleMessageAsync((TClient)client, (TMessage)message);
        }
    }
}

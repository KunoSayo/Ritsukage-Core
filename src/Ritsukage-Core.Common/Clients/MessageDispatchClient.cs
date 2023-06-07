using RUCore.Common.Invoking;

namespace RUCore.Common.Clients
{
    /// <summary>
    /// Message client
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class MessageDispatchClient<TMessage> : MessageClient, IMessageDispatchClient<TMessage> where TMessage : IMessage
    {
        /// <summary>
        /// Dispatches the message to all handlers.
        /// </summary>
        /// <typeparam name="TRelatedMessage"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract Task DispatchAsync<TRelatedMessage>(TRelatedMessage message) where TRelatedMessage : TMessage;
    }
}

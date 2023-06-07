using RUCore.Common.Invoking;

namespace RUCore.Common.Clients
{
    /// <summary>
    /// Message client interface
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMessageDispatchClient<TMessage> : IMessageClient where TMessage : IMessage
    {
        /// <summary>
        /// Dispatches the message to all handlers.
        /// </summary>
        /// <typeparam name="TRelatedMessage"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        Task DispatchAsync<TRelatedMessage>(TRelatedMessage message) where TRelatedMessage : TMessage;
    }
}

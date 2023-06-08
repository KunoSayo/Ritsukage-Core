using RUCore.Common.Clients;
using RUCore.Common.Invoking;

namespace RUCore.Common.Handlers
{
    /// <summary>
    /// Message handler interface
    /// </summary>
    public interface IMessageHandler
    {
        /// <summary>
        /// Handle message
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        Task HandleMessageAsync(IMessageClient client, IMessage message)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Message handler interface
    /// </summary>
    /// <typeparam name="TMessageClient"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMessageHandler<in TMessageClient, in TMessage> : IMessageHandler
        where TMessageClient : IMessageClient
        where TMessage : IMessage
    {
        /// <summary>
        /// Handle message
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        Task HandleMessageAsync(TMessageClient client, TMessage message)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Message handler
    /// </summary>
    /// <typeparam name="TMessageClient"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class MessageHandler<TMessageClient, TMessage> : IMessageHandler<TMessageClient, TMessage>
        where TMessageClient : IMessageClient
        where TMessage : IMessage
    {
        /// <summary>
        /// Handle message
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public abstract Task HandleMessageAsync(TMessageClient client, TMessage message);

        /// <summary>
        /// Handle message
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public virtual Task HandleMessageAsync(IMessageClient client, IMessage message)
        {
            throw new NotSupportedException();
        }
    }
}

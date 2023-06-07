using RUCore.Common.Clients;

namespace RUCore.Common.Invoking
{
    /// <summary>
    /// Message handler invoker
    /// </summary>
    /// <typeparam name="TClientService"></typeparam>
    public interface IMessageHandlerInvoker<in TClientService> where TClientService : IMessageClient
    {
        /// <summary>
        /// Handle message
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task HandleMessageAsync<TMessage>(TClientService client, TMessage message) where TMessage : IMessage;
    }

    /// <summary>
    /// Handler invoker with rawdata
    /// </summary>
    /// <typeparam name="TClientService"></typeparam>
    /// <typeparam name="TRawdata"></typeparam>
    public interface IMessageHandlerInvoker<in TClientService, TRawdata> : IMessageHandlerInvoker<TClientService> where TClientService : IMessageClient
    {
        /// <summary>
        /// Handle rawdata
        /// </summary>
        /// <param name="client"></param>
        /// <param name="rawdata"></param>
        /// <returns></returns>
        Task HandleRawdataAsync(TClientService client, TRawdata rawdata);
    }

}

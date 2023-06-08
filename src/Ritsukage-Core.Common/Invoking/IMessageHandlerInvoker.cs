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
    /// Handler invoker with raw data
    /// </summary>
    /// <typeparam name="TClientService"></typeparam>
    /// <typeparam name="TRawData"></typeparam>
    public interface IMessageHandlerInvoker<in TClientService, TRawData> : IMessageHandlerInvoker<TClientService>
        where TClientService : IMessageClient
    {
        /// <summary>
        /// Handle raw data
        /// </summary>
        /// <param name="client"></param>
        /// <param name="rawData"></param>
        /// <returns></returns>
        Task HandleRawDataAsync(TClientService client, TRawData rawData);
    }
}

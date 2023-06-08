using RUCore.Common.Clients;
using RUCore.Common.Invoking;

namespace RUCore.Common.Handlers
{
    /// <summary>
    /// Contravariance message handler interface
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public interface IContravarianceMessageHandler<in TClient, in TMessage> : IMessageHandler<TClient, TMessage>
        where TClient : IMessageClient
        where TMessage : IMessage
    {
    }
}

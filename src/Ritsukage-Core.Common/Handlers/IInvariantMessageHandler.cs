using RUCore.Common.Clients;
using RUCore.Common.Invoking;

namespace RUCore.Common.Handlers
{
    /// <summary>
    /// Invariant message handler interface
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public interface IInvariantMessageHandler<TClient, TMessage> : IMessageHandler<TClient, TMessage>
        where TClient : IMessageClient
        where TMessage : IMessage
    {
    }
}

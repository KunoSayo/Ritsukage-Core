using RUCore.Common.Clients;

namespace RUCore.Common.Invoking
{
    /// <summary>
    /// Message subscription.
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TSubscription"></typeparam>
    public interface IMessageSubscriptionResolver<in TClient, out TSubscription> where TClient : IMessageClient
                                                                                 where TSubscription : IMessageSubscription
    {
        /// <summary>
        /// Resolve subscription by client type.
        /// </summary>
        /// <param name="handlerType"></param>
        /// <returns></returns>
        IEnumerable<TSubscription> ResolveByHandler(Type handlerType);

        /// <summary>
        /// Resolve subscription by message type.
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        TSubscription? ResolveByMessage(Type messageType);
    }
}

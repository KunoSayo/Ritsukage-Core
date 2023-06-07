using RUCore.Common.Clients;
using RUCore.Common.Handlers;
using System.Collections.Concurrent;

namespace RUCore.Common.Invoking
{
    /// <summary>
    /// Message subscription resolver.
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TSubscription"></typeparam>
    public class MessageSubscriptionResolver<TClient, TSubscription> : IMessageSubscriptionResolver<TClient, TSubscription> where TClient : IMessageClient
                                                                                                                            where TSubscription : IMessageSubscription
    {
        /// <summary>
        /// Service provider used to resolve subscriptions.
        /// </summary>
        protected readonly IServiceProvider _services;

        /// <summary>
        /// Stores the mapping between message types and subscription types.
        /// </summary>
        protected readonly ConcurrentDictionary<Type, Type> _subscriptionTypeMapping = new();

        /// <summary>
        /// Message subscription resolver.
        /// </summary>
        /// <param name="services"></param>
        public MessageSubscriptionResolver(IServiceProvider services)
        {
            _services = services;
        }

        /// <summary>
        /// Get the subscription type for the given message type.
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        protected virtual Type GetSubscriptionType(Type messageType)
            => typeof(IMessageSubscription<,>).MakeGenericType(typeof(TClient), messageType);

        /// <summary>
        /// Resolve the subscription for the given message type.
        /// </summary>
        /// <param name="handlerType"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual IEnumerable<TSubscription> ResolveByHandler(Type handlerType)
        {
            Type openGeneric = typeof(IMessageHandler<,>);
            List<TSubscription> subscriptions = new List<TSubscription>();
            foreach (Type interfaceType in handlerType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == openGeneric)
                {
                    Type[] genericArguments = interfaceType.GetGenericArguments();
                    Type clientType = genericArguments[0];
                    Type thisClientType = typeof(TClient);
                    if (thisClientType.IsAssignableFrom(genericArguments[0]))
                    {
                        TSubscription? subscription = ResolveByMessage(genericArguments[1]);
                        if (subscription != null)
                            subscriptions.Add(subscription);
                        continue;
                    }
                    throw new InvalidOperationException($"Given handler type {handlerType.FullName} specifies client type {clientType.FullName} which is not compatible with {thisClientType.FullName}.");
                }
            }
            return subscriptions;
        }

        /// <summary>
        /// Resolve the subscription for the given message type.
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public virtual TSubscription? ResolveByMessage(Type messageType)
        {
            if (!_subscriptionTypeMapping.TryGetValue(messageType, out Type? subscriptionType))
                _subscriptionTypeMapping[messageType] = subscriptionType = GetSubscriptionType(messageType);
            return (TSubscription?)_services.GetService(subscriptionType);
        }
    }
}

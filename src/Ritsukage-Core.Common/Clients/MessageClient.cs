using RUCore.Common.Handlers;
using RUCore.Common.Invoking;

namespace RUCore.Common.Clients
{
    /// <summary>
    /// Message client
    /// </summary>
    public abstract class MessageClient : IMessageClient
    {
        /// <summary>
        /// Resolve message subscription by handler type
        /// </summary>
        /// <param name="handlerType"></param>
        /// <returns></returns>
        protected abstract IEnumerable<IMessageSubscription> ResolveByHandler(Type handlerType);

        /// <summary>
        /// Add plugin
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public PluginResistration AddPlugin(IMessageHandler handler)
        {
            LinkedList<DynamicHandlerRegistration> registrations = new();
            foreach (IMessageSubscription subscription in ResolveByHandler(handler.GetType()))
                registrations.AddLast(subscription.AddHandler(handler));
            return new PluginResistration(registrations);
        }
    }
}

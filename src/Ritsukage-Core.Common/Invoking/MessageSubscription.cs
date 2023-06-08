using RUCore.Common.Clients;
using RUCore.Common.Handlers;
using System.Collections;

namespace RUCore.Common.Invoking
{
    /// <summary>
    /// Message subscription.
    /// </summary>
    public abstract partial class MessageSubscription : IMessageSubscription
    {
        /// <summary>
        /// Static handlers.
        /// </summary>
        protected IMessageHandler[] StaticHandlers { get; }

        /// <summary>
        /// Registrations.
        /// </summary>
        protected Registrations? _registrations;

        /// <summary>
        /// Disposed.
        /// </summary>
        protected bool _disposed;

        /// <summary>
        /// Message subscription.
        /// </summary>
        /// <param name="handlers"></param>
        protected MessageSubscription(IEnumerable<IMessageHandler> handlers)
        {
            StaticHandlers = handlers.Any()
                ? ResolveStaticHandlers(new LinkedList<IMessageHandler>(handlers), new List<IMessageHandler>())
                : Array.Empty<IMessageHandler>();
        }

        /// <summary>
        /// Resolve static handlers.
        /// </summary>
        /// <param name="handlers"></param>
        /// <param name="filtered"></param>
        /// <returns></returns>
        protected abstract IMessageHandler[] ResolveStaticHandlers(LinkedList<IMessageHandler> handlers,
                                                                   List<IMessageHandler>       filtered);

        /// <summary>
        /// Add handler.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public abstract DynamicHandlerRegistration AddHandler(IMessageHandler handler);

        /// <summary>
        /// Handle message.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public virtual Task HandleMessageAsync(IMessageClient client, IMessage message)
        {
            throw new NotSupportedException("Please use HandleMessageAsync method in generic interface.");
        }

        /// <summary>
        /// Get enumerator.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<IMessageHandler> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            _disposed = true;
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Message subscription.
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public partial class MessageSubscription<TClient, TMessage> : MessageSubscription,
                                                                  IMessageSubscription<TClient, TMessage>
        where TClient : IMessageClient
        where TMessage : IMessage
    {
        /// <summary>
        /// Message subscription.
        /// </summary>
        /// <param name="handlers"></param>
        public MessageSubscription(IEnumerable<IMessageHandler> handlers) : base(handlers)
        {
        }

        /// <summary>
        /// Resolve static handlers.
        /// </summary>
        /// <param name="handlers"></param>
        /// <param name="filtered"></param>
        /// <returns></returns>
        protected override IMessageHandler[] ResolveStaticHandlers(LinkedList<IMessageHandler> handlers,
                                                                   List<IMessageHandler>       filtered)
        {
            if (handlers.Count != 0)
            {
                for (LinkedListNode<IMessageHandler>? handlerNode = handlers.First;
                     handlerNode != null;
                     handlerNode = handlerNode.Next)
                {
                    IMessageHandler handler = handlerNode.Value;
                    if (handler is IContravariantMessageHandler<TClient, TMessage> or
                                   IInvariantMessageHandler<TClient, TMessage>)
                    {
                        filtered.Add(handler);
                        handlers.Remove(handlerNode);
                    }
                }
            }

            return filtered.ToArray();
        }

        /// <summary>
        /// Add handler.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public override DynamicHandlerRegistration AddHandler(IMessageHandler handler)
        {
            return AddHandler((IMessageHandler<TClient, TMessage>)handler);
        }

        /// <summary>
        /// Add handler.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public virtual DynamicHandlerRegistration AddHandler(IMessageHandler<TClient, TMessage> handler)
        {
            Registrations? registrations = Volatile.Read(ref _registrations);
            if (registrations is null)
            {
                registrations = new Registrations(this);
                registrations = Interlocked.CompareExchange(ref _registrations, registrations, null) ?? registrations;
            }

            RegistrationNode? node = registrations.Register(handler);
            long              id   = node.Id;
            return !_disposed || !registrations.Unregister(id, node)
                ? new DynamicHandlerRegistration(id, node)
                : default;
        }

        /// <summary>
        /// Remove handler.
        /// </summary>
        /// <param name="handler"></param>
        public virtual void RemoveHandler(IMessageHandler<TClient, TMessage> handler)
        {
        }

        /// <summary>
        /// Handle message.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual async Task HandleMessageAsync(TClient client, TMessage message)
        {
            foreach (IMessageHandler<TClient, TMessage> handler in this.Cast<IMessageHandler<TClient, TMessage>>())
            {
                await handler.HandleMessageAsync(client, message);
                if (message.Cancel)
                    break;
            }
        }

        /// <summary>
        /// Implement <see cref="MessageSubscription.HandleMessageAsync(IMessageClient, IMessage)"/> method.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Task HandleMessageAsync(IMessageClient client, IMessage message)
        {
            return HandleMessageAsync((TClient)client, (TMessage)message);
        }
    }
}

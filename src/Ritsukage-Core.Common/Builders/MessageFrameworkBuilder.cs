using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RUCore.Common.Clients;
using RUCore.Common.Handlers;
using RUCore.Common.Invoking;
using RUCore.Common.Invoking.Attributes;
using RUCore.Common.Parsers;
using RUCore.Common.Parsers.Attributes;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace RUCore.Common.Builders
{
    /// <summary>
    /// Base class for message framework builders.
    /// </summary>
    /// <typeparam name="TInvokerService"></typeparam>
    /// <typeparam name="TClientService"></typeparam>
    /// <typeparam name="THandlerService"></typeparam>
    public abstract class MessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> : IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService>
        where TInvokerService : class, IMessageHandlerInvoker<TClientService>
        where TClientService : class, IMessageClient
        where THandlerService : class, IMessageHandler
    {
        /// <summary>
        /// Default lifetime for handlers.
        /// </summary>
        protected virtual ServiceLifetime DefaultHandlerLifetime => ServiceLifetime.Scoped;

        /// <summary>
        /// Default lifetime for invokers.
        /// </summary>
        protected virtual ServiceLifetime DefaultInvokerLifetime => ServiceLifetime.Scoped;

        /// <summary>
        /// Default lifetime for clients.
        /// </summary>
        protected virtual ServiceLifetime DefaultClientLifetime => ServiceLifetime.Scoped;

        /// <summary>
        /// Default lifetime for parsers.
        /// </summary>
        protected virtual ServiceLifetime DefaultSubscriptionLifetime => ServiceLifetime.Scoped;

        /// <summary>
        /// Default lifetime for subscription resolvers.
        /// </summary>
        protected virtual ServiceLifetime DefaultSubscriptionResolverLifetime => DefaultSubscriptionLifetime;

        /// <summary>
        /// Service collection.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Builds the message framework.
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MessageFrameworkBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Tries to add a service to the service collection.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="lifetime"></param>
        /// <param name="addedDescriptor"></param>
        /// <returns></returns>
        protected bool TryAddService(Type serviceType, Type implementationType, ServiceLifetime lifetime, [NotNullWhen(true)] out ServiceDescriptor? addedDescriptor)
        {
            ServiceDescriptor descriptor = new(serviceType, implementationType, lifetime);
            return TryAddService(descriptor, out addedDescriptor);
        }

        /// <summary>
        /// Tries to add a service to the service collection.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationInstance"></param>
        /// <param name="addedDescriptor"></param>
        /// <returns></returns>
        protected bool TryAddService(Type serviceType, object implementationInstance, [NotNullWhen(true)] out ServiceDescriptor? addedDescriptor)
        {
            ServiceDescriptor descriptor = new(serviceType, implementationInstance);
            return TryAddService(descriptor, out addedDescriptor);
        }

        /// <summary>
        /// Tries to add a service to the service collection.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="factory"></param>
        /// <param name="lifetime"></param>
        /// <param name="addedDescriptor"></param>
        /// <returns></returns>
        protected bool TryAddService(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime, [NotNullWhen(true)] out ServiceDescriptor? addedDescriptor)
        {
            ServiceDescriptor descriptor = new(serviceType, factory, lifetime);
            return TryAddService(descriptor, out addedDescriptor);
        }

        /// <summary>
        /// Tries to add a service to the service collection.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="addedDescriptor"></param>
        /// <returns></returns>
        protected bool TryAddService(ServiceDescriptor descriptor, [NotNullWhen(true)] out ServiceDescriptor? addedDescriptor)
        {
            int count = Services.Count;
            Services.TryAddEnumerable(descriptor);
            if (count != Services.Count)
            {
                addedDescriptor = descriptor;
                return true;
            }
            addedDescriptor = null;
            return false;
        }

        /// <summary>
        /// Adds a message handler.
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddHandler<THandler>() where THandler : class, THandlerService
            => AddHandler<THandler>(DefaultHandlerLifetime);

        /// <summary>
        /// Adds a message handler.
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddHandler<THandler>(ServiceLifetime lifetime) where THandler : class, THandlerService
        {
            TryAddService(typeof(THandlerService), typeof(THandler), lifetime, out _);
            return this;
        }

        /// <summary>
        /// Adds a message handler.
        /// </summary>
        /// <param name="handlerInstance"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddHandler(THandlerService handlerInstance)
        {
            TryAddService(typeof(THandlerService), handlerInstance, out _);
            return this;
        }

        /// <summary>
        /// Adds a message handler.
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddHandler<THandler>(Func<IServiceProvider, THandler> factory) where THandler : class, THandlerService
            => AddHandler(factory, DefaultHandlerLifetime);

        /// <summary>
        /// Adds a message handler.
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="factory"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddHandler<THandler>(Func<IServiceProvider, THandler> factory, ServiceLifetime lifetime) where THandler : class, THandlerService
        {
            TryAddService(typeof(THandlerService), factory, lifetime, out _);
            return this;
        }

        /// <summary>
        /// Adds a message invoker.
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddInvoker<TInvoker>() where TInvoker : class, TInvokerService
            => AddInvoker<TInvoker>(DefaultInvokerLifetime);

        /// <summary>
        /// Adds a message invoker.
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddInvoker<TInvoker>(ServiceLifetime lifetime) where TInvoker : class, TInvokerService
        {
            Type invokerType = typeof(TInvoker);
            TryAddService(typeof(TInvokerService), invokerType, lifetime, out _);
            ResolveMessageSubscription(invokerType, lifetime);
            ResolveMessageSubscriptionResolver(invokerType, lifetime);
            return this;
        }

        /// <summary>
        /// Adds a message invoker.
        /// </summary>
        /// <param name="invokerInstance"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddInvoker(TInvokerService invokerInstance)
        {
            Type instanceType = typeof(TInvokerService);
            TryAddService(typeof(TInvokerService), invokerInstance, out _);
            ResolveMessageSubscription(instanceType, ServiceLifetime.Singleton);
            ResolveMessageSubscriptionResolver(instanceType, ServiceLifetime.Singleton);
            return this;
        }

        /// <summary>
        /// Adds a message invoker.
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddInvoker<TInvoker>(Func<IServiceProvider, TInvoker> factory) where TInvoker : class, TInvokerService
            => AddInvoker(factory, DefaultInvokerLifetime);

        /// <summary>
        /// Adds a message invoker.
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="factory"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddInvoker<TInvoker>(Func<IServiceProvider, TInvoker> factory, ServiceLifetime lifetime) where TInvoker : class, TInvokerService
        {
            Type invokerType = typeof(TInvokerService);
            TryAddService(invokerType, factory, lifetime, out _);
            ResolveMessageSubscription(invokerType, lifetime);
            ResolveMessageSubscriptionResolver(invokerType, lifetime);
            return this;
        }

        /// <summary>
        /// Adds a message client.
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddClient<TClient>() where TClient : class, TClientService
            => AddClient<TClient>(DefaultClientLifetime);

        /// <summary>
        /// Adds a message client.
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddClient<TClient>(ServiceLifetime lifetime) where TClient : class, TClientService
        {
            TryAddService(typeof(TClientService), typeof(TClient), lifetime, out _);
            return this;
        }

        /// <summary>
        /// Adds a message client.
        /// </summary>
        /// <param name="clientInstance"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddClient(TClientService clientInstance)
        {
            TryAddService(typeof(TClientService), clientInstance, out _);
            return this;
        }

        /// <summary>
        /// Adds a message client.
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddClient<TClient>(Func<IServiceProvider, TClient> factory) where TClient : class, TClientService
            => AddClient(factory, DefaultClientLifetime);

        /// <summary>
        /// Adds a message client.
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="factory"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddClient<TClient>(Func<IServiceProvider, TClient> factory, ServiceLifetime lifetime) where TClient : class, TClientService
        {
            TryAddService(typeof(TClientService), factory, lifetime, out _);
            return this;
        }

        /// <summary>
        /// Resolve message subscription.
        /// </summary>
        /// <param name="invokerType"></param>
        /// <param name="lifetime"></param>
        protected virtual void ResolveMessageSubscription(Type invokerType, ServiceLifetime? lifetime)
        {
            IEnumerable<RegisterMessageSubscriptionAttribute> attributes = invokerType.GetCustomAttributes<RegisterMessageSubscriptionAttribute>(false);
            foreach (RegisterMessageSubscriptionAttribute attribute in attributes)
                Services.Add(new ServiceDescriptor(attribute.ServiceType, attribute.ImplementationType, lifetime ?? attribute.Lifetime ?? DefaultSubscriptionLifetime));
        }

        /// <summary>
        /// Resolve message subscription resolver.
        /// </summary>
        /// <param name="invokerType"></param>
        /// <param name="lifetime"></param>
        protected virtual void ResolveMessageSubscriptionResolver(Type invokerType, ServiceLifetime? lifetime)
        {
            IEnumerable<RegisterMessageSubscriptionResolverAttribute> attributes = invokerType.GetCustomAttributes<RegisterMessageSubscriptionResolverAttribute>(false);
            foreach (RegisterMessageSubscriptionResolverAttribute attribute in attributes)
                Services.Add(new ServiceDescriptor(attribute.ServiceType, attribute.ImplementationType, lifetime ?? attribute.Lifetime ?? DefaultSubscriptionResolverLifetime));
        }

        /// <summary>
        /// Replaces the service lifetime.
        /// </summary>
        /// <param name="addedDescriptors"></param>
        /// <param name="lifetime"></param>
        protected virtual void ReplaceServiceLifetime(ICollection<ServiceDescriptor> addedDescriptors, ServiceLifetime lifetime)
        {
            for (int i = Services.Count - 1; i >= 0; i--)
            {
                ServiceDescriptor descriptor = Services[i];
                if (addedDescriptors.Contains(descriptor) && descriptor.ImplementationInstance == null)
                {
                    Services.RemoveAt(i);
                    addedDescriptors.Remove(descriptor);
                    if (descriptor.ImplementationFactory != null)
                        descriptor = new ServiceDescriptor(descriptor.ServiceType, descriptor.ImplementationFactory, lifetime);
                    else
                        descriptor = new ServiceDescriptor(descriptor.ServiceType, descriptor.ImplementationType!, lifetime);
                    Services.Add(descriptor);
                    addedDescriptors.Add(descriptor);
                }
            }
        }
    }

    /// <summary>
    /// Message framework builder.
    /// </summary>
    /// <typeparam name="TInvokerService"></typeparam>
    /// <typeparam name="TClientService"></typeparam>
    /// <typeparam name="THandlerService"></typeparam>
    /// <typeparam name="TParserService"></typeparam>
    public abstract class MessageFrameworkBuilder<TInvokerService, TClientService, THandlerService, TParserService> : MessageFrameworkBuilder<TInvokerService, TClientService, THandlerService>, IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService, TParserService>
        where TParserService : class, IMessageParser
        where THandlerService : class, IMessageHandler
        where TInvokerService : class, IMessageHandlerInvoker<TClientService>
        where TClientService : class, IMessageClient
    {
        /// <summary>
        /// Default parser lifetime.
        /// </summary>
        protected virtual ServiceLifetime DefaultParserLifetime => ServiceLifetime.Singleton;

        /// <summary>
        /// Default parser resolver lifetime.
        /// </summary>
        protected virtual ServiceLifetime DefaultParserResolverLifetime => DefaultParserLifetime;

        /// <summary>
        /// Max parser lifetime.
        /// </summary>
        protected ServiceLifetime _maxParserLifetime;

        /// <summary>
        /// Max parser resolver lifetime.
        /// </summary>
        protected ServiceLifetime _maxParserResolverLifetime;

        /// <summary>
        /// Added parser resolvers.
        /// </summary>
        protected HashSet<ServiceDescriptor> _addedParserResolvers;

        /// <summary>
        /// Message framework builder.
        /// </summary>
        /// <param name="services"></param>
        public MessageFrameworkBuilder(IServiceCollection services) : base(services)
        {
            _addedParserResolvers = new HashSet<ServiceDescriptor>();
        }

        /// <summary>
        /// Adds a message parser.
        /// </summary>
        /// <typeparam name="TParser"></typeparam>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService, TParserService> AddParser<TParser>() where TParser : class, TParserService
            => AddParser<TParser>(DefaultParserLifetime);

        /// <summary>
        /// Adds a message parser.
        /// </summary>
        /// <typeparam name="TParser"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService, TParserService> AddParser<TParser>(ServiceLifetime lifetime) where TParser : class, TParserService
        {
            TryAddService(typeof(TParserService), typeof(TParser), lifetime, out _);
            return this;
        }

        /// <summary>
        /// Adds a message parser.
        /// </summary>
        /// <param name="parserInstance"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService, TParserService> AddParser(TParserService parserInstance)
        {
            TryAddService(typeof(TParserService), parserInstance, out _);
            return this;
        }

        /// <summary>
        /// Adds a message parser.
        /// </summary>
        /// <typeparam name="TParser"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService, TParserService> AddParser<TParser>(Func<IServiceProvider, TParser> factory) where TParser : class, TParserService
            => AddParser(factory, DefaultParserLifetime);

        /// <summary>
        /// Adds a message parser.
        /// </summary>
        /// <typeparam name="TParser"></typeparam>
        /// <param name="factory"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public virtual IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService, TParserService> AddParser<TParser>(Func<IServiceProvider, TParser> factory, ServiceLifetime lifetime) where TParser : class, TParserService
        {
            TryAddService(typeof(TParserService), factory, lifetime, out _);
            return this;
        }

        /// <summary>
        /// Adds a message handler.
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public override IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddHandler<THandler>(ServiceLifetime lifetime)
        {
            base.AddHandler<THandler>(lifetime);
            ResolveMessageParser(typeof(THandler), lifetime);
            return this;
        }

        /// <summary>
        /// Adds a message handler.
        /// </summary>
        /// <param name="handlerInstance"></param>
        /// <returns></returns>
        public override IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddHandler(THandlerService handlerInstance)
        {
            base.AddHandler(handlerInstance);
            ResolveMessageParser(handlerInstance.GetType(), ServiceLifetime.Singleton);
            return this;
        }

        /// <summary>
        /// Adds a message handler.
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="factory"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public override IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddHandler<THandler>(Func<IServiceProvider, THandler> factory, ServiceLifetime lifetime)
        {
            base.AddHandler(factory, lifetime);
            ResolveMessageParser(typeof(THandler), lifetime);
            return this;
        }

        /// <summary>
        /// Adds a message invoker.
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public override IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddInvoker<TInvoker>(ServiceLifetime lifetime)
        {
            base.AddInvoker<TInvoker>(lifetime);
            ResolveMessageParserResolver(typeof(TInvoker), _maxParserLifetime);
            return this;
        }

        /// <summary>
        /// Adds a message invoker.
        /// </summary>
        /// <param name="invokerInstance"></param>
        /// <returns></returns>
        public override IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddInvoker(TInvokerService invokerInstance)
        {
            base.AddInvoker(invokerInstance);
            ResolveMessageParserResolver(invokerInstance.GetType(), ServiceLifetime.Singleton);
            return this;
        }

        /// <summary>
        /// Adds a message invoker.
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="factory"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public override IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddInvoker<TInvoker>(Func<IServiceProvider, TInvoker> factory, ServiceLifetime lifetime)
        {
            base.AddInvoker(factory, lifetime);
            ResolveMessageParserResolver(typeof(TInvoker), _maxParserLifetime);
            return this;
        }

        /// <summary>
        /// Resolves the message parser.
        /// </summary>
        /// <param name="handlerType"></param>
        /// <param name="lifetime"></param>
        protected virtual void ResolveMessageParser(Type handlerType, ServiceLifetime? lifetime)
        {
            foreach (RegisterParserAttribute attribute in handlerType.GetCustomAttributes<RegisterParserAttribute>(false))
            {
                ServiceLifetime parserLifetime = lifetime ?? attribute.Lifetime ?? DefaultParserLifetime;
                AddMessageParser(attribute.ServiceType, attribute.ImplementationType, parserLifetime);
            }
        }

        /// <summary>
        /// Resolves the message parser resolver.
        /// </summary>
        /// <param name="invokerType"></param>
        /// <param name="lifetime"></param>
        protected virtual void ResolveMessageParserResolver(Type invokerType, ServiceLifetime? lifetime)
        {
            foreach (RegisterParserResolverAttribute attribute in invokerType.GetCustomAttributes<RegisterParserResolverAttribute>(false))
            {
                ServiceLifetime parserResolverLifetime = lifetime ?? attribute.Lifetime ?? DefaultParserResolverLifetime;
                AddMessageParserResolver(attribute.ServiceType, attribute.ImplementationType, parserResolverLifetime);
            }
        }

        /// <summary>
        /// Adds a message parser.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="lifetime"></param>
        protected void AddMessageParser(Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            if (TryAddService(serviceType, implementationType, lifetime, out _))
            {
                if (lifetime > _maxParserLifetime)
                {
                    _maxParserLifetime = lifetime;
                    if (lifetime > _maxParserResolverLifetime)
                    {
                        _maxParserResolverLifetime = lifetime;
                        ReplaceServiceLifetime(_addedParserResolvers, lifetime);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a message parser resolver.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="lifetime"></param>
        protected void AddMessageParserResolver(Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            if (TryAddService(serviceType, implementationType, lifetime, out ServiceDescriptor? addedDescriptor))
            {
                if (lifetime > _maxParserResolverLifetime)
                    _maxParserResolverLifetime = lifetime;
                _addedParserResolvers.Add(addedDescriptor!);
            }
        }
    }
}

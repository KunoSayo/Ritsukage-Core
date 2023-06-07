using Microsoft.Extensions.Logging;
using RUCore.Common.Clients;
using RUCore.Common.Parsers;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RUCore.Common.Invoking
{
    /// <summary>
    /// Message handler invoker.
    /// </summary>
    /// <typeparam name="TClientService"></typeparam>
    public abstract class MessageHandlerInvoker<TClientService> : IMessageHandlerInvoker<TClientService> where TClientService : IMessageClient
    {
        /// <summary>
        /// Services.
        /// </summary>
        protected readonly IServiceProvider _Services;

        /// <summary>
        /// Logger.
        /// </summary>
        protected readonly ILogger<MessageHandlerInvoker<TClientService>> _Logger;

        /// <summary>
        /// Subscription resolver.
        /// </summary>
        protected readonly IMessageSubscriptionResolver<TClientService, IMessageSubscription> _subscriptionResolver;

        /// <summary>
        /// Message handler invoker.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="logger"></param>
        /// <param name="subscriptionResolver"></param>
        protected MessageHandlerInvoker(IServiceProvider services,
                                        ILogger<MessageHandlerInvoker<TClientService>> logger,
                                        IMessageSubscriptionResolver<TClientService, IMessageSubscription> subscriptionResolver)
        {
            _Services = services;
            _Logger = logger;
            _subscriptionResolver = subscriptionResolver;
        }
        
        /// <summary>
        /// Try to resolve subscription for the message type.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual Task HandleMessageAsync<TMessage>(TClientService client, TMessage message) where TMessage : IMessage
        {
            if (TryResolveSubscription(out IMessageSubscription<TClientService, TMessage>? subscription))
                return subscription!.HandleMessageAsync(client, message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Try to resolve subscription for the message type.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="subscription"></param>
        /// <returns></returns>
        protected virtual bool TryResolveSubscription<TMessage>([NotNullWhen(true)] out IMessageSubscription<TClientService, TMessage>? subscription) where TMessage : IMessage
        {
            Unsafe.SkipInit(out subscription);
            ref IMessageSubscription? s = ref Unsafe.As<IMessageSubscription<TClientService, TMessage>?, IMessageSubscription?>(ref subscription);
            return TryResolveSubscription(typeof(TMessage), out s);
        }

        /// <summary>
        /// Try to resolve subscription for the message type.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="subscription"></param>
        /// <returns></returns>
        protected virtual bool TryResolveSubscription(Type messageType, [NotNullWhen(true)] out IMessageSubscription? subscription)
            => (subscription = _subscriptionResolver.ResolveByMessage(messageType)) != null;
    }

    /// <summary>
    /// Message handler invoker.
    /// </summary>
    /// <typeparam name="TClientService"></typeparam>
    /// <typeparam name="TRawdata"></typeparam>
    public abstract class MessageHandlerInvoker<TClientService, TRawdata> : MessageHandlerInvoker<TClientService>, IMessageHandlerInvoker<TClientService, TRawdata> where TClientService : IMessageClient
    {
        /// <summary>
        /// Subscription mapping.
        /// </summary>
        protected readonly ConcurrentDictionary<Type, Type> _SubscriptionMapping = new();

        /// <summary>
        /// Parser resolver.
        /// </summary>
        protected readonly IMessageParserResolver<TRawdata, IMessageParser<TRawdata>> _parserResolver;

        /// <summary>
        /// Parser resolver.
        /// </summary>
        protected virtual IMessageParserResolver<TRawdata, IMessageParser<TRawdata>> ParserResolver => _parserResolver;

        /// <summary>
        /// Message handler invoker.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="logger"></param>
        /// <param name="subscriptionResolver"></param>
        /// <param name="parserResolver"></param>
        protected MessageHandlerInvoker(IServiceProvider services,
                                        ILogger<MessageHandlerInvoker<TClientService, TRawdata>> logger,
                                        IMessageSubscriptionResolver<TClientService, IMessageSubscription> subscriptionResolver,
                                        IMessageParserResolver<TRawdata, IMessageParser<TRawdata>> parserResolver) : base(services, logger, subscriptionResolver)
        {
            _parserResolver = parserResolver;
        }

        /// <summary>
        /// Handle rawdata.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="rawdata"></param>
        /// <returns></returns>
        public virtual async Task HandleRawdataAsync(TClientService client, TRawdata rawdata)
        {
            if (TryResolveParsers(in rawdata, out var parsers))
            {
                foreach (var parser in parsers!)
                {
                    if (parser.CanParse(in rawdata))
                    {
                        IMessage<TRawdata> message = parser.Parse(in rawdata);
                        if (TryResolveSubscription(parser.MessageType, out IMessageSubscription? subscription))
                        {
                            await subscription!.HandleMessageAsync(client, message).ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Try to resolve parsers for the rawdata.
        /// </summary>
        /// <param name="rawdata"></param>
        /// <param name="parsers"></param>
        /// <returns></returns>
        protected virtual bool TryResolveParsers(in TRawdata rawdata, [NotNullWhen(true)] out IEnumerable<IMessageParser<TRawdata>>? parsers)
        {
            parsers = ParserResolver.ResolveParsers(in rawdata);
            return parsers != null;
        }
    }
}

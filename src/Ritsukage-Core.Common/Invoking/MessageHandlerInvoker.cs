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
    public abstract class MessageHandlerInvoker<TClientService> : IMessageHandlerInvoker<TClientService>
        where TClientService : IMessageClient
    {
        /// <summary>
        /// Services.
        /// </summary>
        protected readonly IServiceProvider Services;

        /// <summary>
        /// Logger.
        /// </summary>
        protected readonly ILogger<MessageHandlerInvoker<TClientService>> Logger;

        /// <summary>
        /// Subscription resolver.
        /// </summary>
        protected readonly IMessageSubscriptionResolver<TClientService, IMessageSubscription> SubscriptionResolver;

        /// <summary>
        /// Message handler invoker.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="logger"></param>
        /// <param name="subscriptionResolver"></param>
        protected MessageHandlerInvoker(IServiceProvider                               services,
                                        ILogger<MessageHandlerInvoker<TClientService>> logger,
                                        IMessageSubscriptionResolver<TClientService, IMessageSubscription>
                                            subscriptionResolver)
        {
            Services             = services;
            Logger               = logger;
            SubscriptionResolver = subscriptionResolver;
        }

        /// <summary>
        /// Try to resolve subscription for the message type.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual Task HandleMessageAsync<TMessage>(TClientService client, TMessage message)
            where TMessage : IMessage
        {
            return TryResolveSubscription(out IMessageSubscription<TClientService, TMessage>? subscription)
                ? subscription!.HandleMessageAsync(client, message)
                : Task.CompletedTask;
        }

        /// <summary>
        /// Try to resolve subscription for the message type.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="subscription"></param>
        /// <returns></returns>
        protected virtual bool TryResolveSubscription<TMessage>(
            [NotNullWhen(true)] out IMessageSubscription<TClientService, TMessage>? subscription)
            where TMessage : IMessage
        {
            Unsafe.SkipInit(out subscription);
            ref IMessageSubscription? s =
                ref Unsafe.As<IMessageSubscription<TClientService, TMessage>?, IMessageSubscription?>(ref subscription);
            return TryResolveSubscription(typeof(TMessage), out s);
        }

        /// <summary>
        /// Try to resolve subscription for the message type.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="subscription"></param>
        /// <returns></returns>
        protected virtual bool TryResolveSubscription(Type                                          messageType,
                                                      [NotNullWhen(true)] out IMessageSubscription? subscription)
        {
            return (subscription = SubscriptionResolver.ResolveByMessage(messageType)) != null;
        }
    }

    /// <summary>
    /// Message handler invoker.
    /// </summary>
    /// <typeparam name="TClientService"></typeparam>
    /// <typeparam name="TRawData"></typeparam>
    public abstract class MessageHandlerInvoker<TClientService, TRawData> : MessageHandlerInvoker<TClientService>,
                                                                            IMessageHandlerInvoker<TClientService,
                                                                                TRawData>
        where TClientService : IMessageClient
    {
        /// <summary>
        /// Subscription mapping.
        /// </summary>
        protected readonly ConcurrentDictionary<Type, Type> SubscriptionMapping = new();

        /// <summary>
        /// Parser resolver.
        /// </summary>
        protected readonly IMessageParserResolver<TRawData, IMessageParser<TRawData>> InnerParserResolver;

        /// <summary>
        /// Parser resolver.
        /// </summary>
        protected virtual IMessageParserResolver<TRawData, IMessageParser<TRawData>> ParserResolver => InnerParserResolver;

        /// <summary>
        /// Message handler invoker.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="logger"></param>
        /// <param name="subscriptionResolver"></param>
        /// <param name="parserResolver"></param>
        protected MessageHandlerInvoker(IServiceProvider                                         services,
                                        ILogger<MessageHandlerInvoker<TClientService, TRawData>> logger,
                                        IMessageSubscriptionResolver<TClientService, IMessageSubscription>
                                            subscriptionResolver,
                                        IMessageParserResolver<TRawData, IMessageParser<TRawData>> parserResolver) :
            base(services, logger, subscriptionResolver)
        {
            InnerParserResolver = parserResolver;
        }

        /// <summary>
        /// Handle raw data.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public virtual async Task HandleRawDataAsync(TClientService client, TRawData rawData)
        {
            if (TryResolveParsers(in rawData, out var parsers))
            {
                foreach (var parser in parsers!)
                {
                    if (parser.CanParse(in rawData))
                    {
                        IMessage<TRawData> message = parser.Parse(in rawData);
                        if (TryResolveSubscription(parser.MessageType, out IMessageSubscription? subscription))
                        {
                            await subscription!.HandleMessageAsync(client, message).ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Try to resolve parsers for the raw data.
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="parsers"></param>
        /// <returns></returns>
        protected virtual bool TryResolveParsers(in                      TRawData                               rawData,
                                                 [NotNullWhen(true)] out IEnumerable<IMessageParser<TRawData>>? parsers)
        {
            parsers = ParserResolver.ResolveParsers(in rawData);
            return parsers != null;
        }
    }
}

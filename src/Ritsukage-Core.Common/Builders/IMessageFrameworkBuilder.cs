using Microsoft.Extensions.DependencyInjection;
using RUCore.Common.Clients;
using RUCore.Common.Handlers;
using RUCore.Common.Invoking;
using RUCore.Common.Parsers;

namespace RUCore.Common.Builders
{
    /// <summary>
    /// Message framework builder interface
    /// </summary>
    /// <typeparam name="TInvokerService"></typeparam>
    /// <typeparam name="TClientService"></typeparam>
    /// <typeparam name="THandlerService"></typeparam>
    public interface IMessageFrameworkBuilder<in TInvokerService, in TClientService, in THandlerService>
        where TInvokerService : class, IMessageHandlerInvoker<TClientService>
        where TClientService : class, IMessageClient
        where THandlerService : class, IMessageHandler
    {
        /// <summary>
        /// Service collection
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Add message handler
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddHandler<THandler>()
            where THandler : class, THandlerService;

        /// <summary>
        /// Add message handler
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService>
            AddHandler<THandler>(ServiceLifetime lifetime) where THandler : class, THandlerService;

        /// <summary>
        /// Add message handler
        /// </summary>
        /// <param name="handlerInstance"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddHandler(
            THandlerService handlerInstance);

        /// <summary>
        /// Add message handler
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddHandler<THandler>(
            Func<IServiceProvider, THandler> factory) where THandler : class, THandlerService;

        /// <summary>
        /// Add message handler
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="factory"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddHandler<THandler>(
            Func<IServiceProvider, THandler> factory, ServiceLifetime lifetime) where THandler : class, THandlerService;

        /// <summary>
        /// Add message invoker
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddInvoker<TInvoker>()
            where TInvoker : class, TInvokerService;

        /// <summary>
        /// Add message invoker
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService>
            AddInvoker<TInvoker>(ServiceLifetime lifetime) where TInvoker : class, TInvokerService;

        /// <summary>
        /// Add message invoker
        /// </summary>
        /// <param name="invokerInstance"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddInvoker(
            TInvokerService invokerInstance);

        /// <summary>
        /// Add message invoker
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddInvoker<TInvoker>(
            Func<IServiceProvider, TInvoker> factory) where TInvoker : class, TInvokerService;

        /// <summary>
        /// Add message invoker
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="factory"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddInvoker<TInvoker>(
            Func<IServiceProvider, TInvoker> factory, ServiceLifetime lifetime) where TInvoker : class, TInvokerService;

        /// <summary>
        /// Add message client
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddClient<TClient>()
            where TClient : class, TClientService;

        /// <summary>
        /// Add message client
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService>
            AddClient<TClient>(ServiceLifetime lifetime) where TClient : class, TClientService;

        /// <summary>
        /// Add message client
        /// </summary>
        /// <param name="clientInstance"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddClient(
            TClientService clientInstance);

        /// <summary>
        /// Add message client
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddClient<TClient>(
            Func<IServiceProvider, TClient> factory) where TClient : class, TClientService;

        /// <summary>
        /// Add message client
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="factory"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService> AddClient<TClient>(
            Func<IServiceProvider, TClient> factory, ServiceLifetime lifetime) where TClient : class, TClientService;
    }


    /// <summary>
    /// Message framework builder interface
    /// </summary>
    /// <typeparam name="TInvokerService"></typeparam>
    /// <typeparam name="TClientService"></typeparam>
    /// <typeparam name="THandlerService"></typeparam>
    /// <typeparam name="TParserService"></typeparam>
    public interface IMessageFrameworkBuilder<in TInvokerService, in TClientService, in THandlerService,
                                              in TParserService> : IMessageFrameworkBuilder<TInvokerService,
        TClientService, THandlerService>
        where TInvokerService : class, IMessageHandlerInvoker<TClientService>
        where TClientService : class, IMessageClient
        where THandlerService : class, IMessageHandler
        where TParserService : class, IMessageParser
    {
        /// <summary>
        /// Add message parser
        /// </summary>
        /// <typeparam name="TParser"></typeparam>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService, TParserService> AddParser<TParser>()
            where TParser : class, TParserService;

        /// <summary>
        /// Add message parser
        /// </summary>
        /// <typeparam name="TParser"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService, TParserService>
            AddParser<TParser>(ServiceLifetime lifetime) where TParser : class, TParserService;

        /// <summary>
        /// Add message parser
        /// </summary>
        /// <param name="parserInstance"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService, TParserService> AddParser(
            TParserService parserInstance);

        /// <summary>
        /// Add message parser
        /// </summary>
        /// <typeparam name="TParser"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService, TParserService> AddParser<TParser>(
            Func<IServiceProvider, TParser> factory) where TParser : class, TParserService;

        /// <summary>
        /// Add message parser
        /// </summary>
        /// <typeparam name="TParser"></typeparam>
        /// <param name="factory"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        IMessageFrameworkBuilder<TInvokerService, TClientService, THandlerService, TParserService> AddParser<TParser>(
            Func<IServiceProvider, TParser> factory, ServiceLifetime lifetime) where TParser : class, TParserService;
    }
}

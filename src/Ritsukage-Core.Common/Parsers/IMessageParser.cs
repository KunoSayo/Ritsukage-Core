using RUCore.Common.Invoking;

namespace RUCore.Common.Parsers
{
    /// <summary>
    /// Message parser interface
    /// </summary>
    public interface IMessageParser
    {
        /// <summary>
        /// Message type
        /// </summary>
        Type MessageType { get; }
    }

    /// <summary>
    /// Message parser interface
    /// </summary>
    /// <typeparam name="TRawdata"></typeparam>
    public interface IMessageParser<TRawdata> : IMessageParser
    {
        /// <summary>
        /// Check if can parse message from raw data
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        bool CanParse(in TRawdata root);

        /// <summary>
        /// Parse message from raw data
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        IMessage<TRawdata> Parse(in TRawdata root)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Message parser interface
    /// </summary>
    /// <typeparam name="TRawdata"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMessageParser<TRawdata, TMessage> : IMessageParser<TRawdata> where TMessage : IMessage<TRawdata>
    {
        /// <inheritdoc/>
        Type IMessageParser.MessageType => typeof(TMessage);

        /// <summary>
        /// Parse message from raw data
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        new TMessage Parse(in TRawdata root);

        /// <inheritdoc/>
        IMessage<TRawdata> IMessageParser<TRawdata>.Parse(in TRawdata root)
            => Parse(in root);
    }
}

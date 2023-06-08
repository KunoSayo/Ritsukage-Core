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
    /// <typeparam name="TRawData"></typeparam>
    public interface IMessageParser<TRawData> : IMessageParser
    {
        /// <summary>
        /// Check if can parse message from raw data
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        bool CanParse(in TRawData root);

        /// <summary>
        /// Parse message from raw data
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        IMessage<TRawData> Parse(in TRawData root)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Message parser interface
    /// </summary>
    /// <typeparam name="TRawData"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMessageParser<TRawData, TMessage> : IMessageParser<TRawData> where TMessage : IMessage<TRawData>
    {
        /// <inheritdoc/>
        Type IMessageParser.MessageType => typeof(TMessage);

        /// <summary>
        /// Parse message from raw data
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        new TMessage Parse(in TRawData root);

        /// <inheritdoc/>
        IMessage<TRawData> IMessageParser<TRawData>.Parse(in TRawData root)
        {
            return Parse(in root);
        }
    }
}

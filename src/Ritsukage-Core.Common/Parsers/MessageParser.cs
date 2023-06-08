using RUCore.Common.Invoking;

namespace RUCore.Common.Parsers
{
    /// <summary>
    /// Message parser
    /// </summary>
    /// <typeparam name="TRawData"></typeparam>
    public abstract class MessageParser<TRawData> : IMessageParser<TRawData>
    {
        /// <inheritdoc/>
        public abstract Type MessageType { get; }

        /// <inheritdoc/>
        public abstract bool CanParse(in TRawData root);
    }

    /// <summary>
    /// Message parser
    /// </summary>
    /// <typeparam name="TRawData"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class MessageParser<TRawData, TMessage> : MessageParser<TRawData>,
                                                              IMessageParser<TRawData, TMessage>
        where TMessage : IMessage<TRawData>
    {
        /// <inheritdoc/>
        public override Type MessageType => typeof(TMessage);

        /// <inheritdoc/>
        public abstract TMessage Parse(in TRawData root);

        IMessage<TRawData> IMessageParser<TRawData>.Parse(in TRawData root)
        {
            return Parse(in root);
        }
    }
}

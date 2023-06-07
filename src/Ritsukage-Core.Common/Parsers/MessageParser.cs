using RUCore.Common.Invoking;

namespace RUCore.Common.Parsers
{
    /// <summary>
    /// Message parser
    /// </summary>
    /// <typeparam name="TRawdata"></typeparam>
    public abstract class MessageParser<TRawdata> : IMessageParser<TRawdata>
    {
        /// <inheritdoc/>
        public abstract Type MessageType { get; }

        /// <inheritdoc/>
        public abstract bool CanParse(in TRawdata root);
    }

    /// <summary>
    /// Message parser
    /// </summary>
    /// <typeparam name="TRawdata"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class MessageParser<TRawdata, TMessage> : MessageParser<TRawdata>, IMessageParser<TRawdata, TMessage> where TMessage : IMessage<TRawdata>
    {
        /// <inheritdoc/>
        public override Type MessageType => typeof(TMessage);

        /// <inheritdoc/>
        public abstract TMessage Parse(in TRawdata root);

        IMessage<TRawdata> IMessageParser<TRawdata>.Parse(in TRawdata root)
            => Parse(in root);
    }
}

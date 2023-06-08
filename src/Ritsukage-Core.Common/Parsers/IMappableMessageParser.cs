using RUCore.Common.Invoking;

namespace RUCore.Common.Parsers
{
    /// <summary>
    /// Message parser
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IMappableMessageParser<TKey>
    {
        /// <summary>
        /// Key
        /// </summary>
        TKey Key { get; }
    }

    /// <summary>
    /// Message parser
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TRawData"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMappableMessageParser<TKey, TRawData, TMessage> : IMappableMessageParser<TKey>,
                                                                        IMessageParser<TRawData, TMessage>
        where TMessage : IMessage<TRawData>
    {
    }
}

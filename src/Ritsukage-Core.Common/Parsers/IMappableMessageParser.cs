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
    /// <typeparam name="TRawdata"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMappableMessageParser<TKey, TRawdata, TMessage> : IMappableMessageParser<TKey>,
                                                                        IMessageParser<TRawdata, TMessage> where TMessage : IMessage<TRawdata>
    { }
}

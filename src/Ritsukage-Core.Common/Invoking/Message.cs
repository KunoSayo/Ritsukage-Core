namespace RUCore.Common.Invoking
{
    
    /// <summary>
    /// Message
    /// </summary>
    public abstract class Message : IMessage
    {
        /// <inheritdoc/>
        public virtual bool Cancel { get; set; }
    }

    /// <summary>
    /// Message
    /// </summary>
    /// <typeparam name="TRawData"></typeparam>
    public abstract class Message<TRawData> : Message, IMessage<TRawData>
    {
        /// <inheritdoc/>
        public virtual TRawData RawData { get; set; } = default!;
    }
}

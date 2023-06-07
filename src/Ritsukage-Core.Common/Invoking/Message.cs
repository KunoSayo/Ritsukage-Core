using RUCore.Common.Invoking;

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

    /// <inheritdoc/>
    /// <typeparam name="TRawdata"></typeparam>
    public abstract class Message<TRawdata> : Message, IMessage<TRawdata>
    {
        /// <inheritdoc/>
        public virtual TRawdata Rawdata { get; set; } = default!;
    }
}

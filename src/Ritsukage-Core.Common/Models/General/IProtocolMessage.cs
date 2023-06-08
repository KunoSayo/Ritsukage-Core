using RUCore.Common.Invoking;

namespace RUCore.Common.Models.General
{
    /// <summary>
    /// Interface for protocol message
    /// </summary>
    public interface IProtocolMessage : IMessage
    {
        /// <summary>
        /// Message Id
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Time
        /// </summary>
        DateTime Time { get; }
    }

    /// <summary>
    /// Interface for protocol message
    /// </summary>
    /// <typeparam name="TRawData"></typeparam>
    public interface IProtocolMessage<TRawData> : IProtocolMessage, IMessage<TRawData>
    {
    }
}

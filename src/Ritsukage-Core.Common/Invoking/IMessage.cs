namespace RUCore.Common.Invoking
{
    /// <summary>
    /// Message interface
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// block message
        /// </summary>
        bool Cancel { get; set; }
    }

    /// <summary>
    /// Message interface
    /// </summary>
    /// <typeparam name="TRawData"></typeparam>
    public interface IMessage<TRawData> : IMessage
    {
        /// <summary>
        /// RawData
        /// </summary>
        TRawData RawData { get; }
    }
}

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
    /// <typeparam name="TRawdata"></typeparam>

    public interface IMessage<TRawdata> : IMessage
    {
        /// <summary>
        /// Rawdata
        /// </summary>
        TRawdata Rawdata { get; }
    }
}

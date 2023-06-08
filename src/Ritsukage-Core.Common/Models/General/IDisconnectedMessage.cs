namespace RUCore.Common.Models.General
{
    /// <summary>
    /// Defines a message that is sent when the connection to the server is lost.
    /// </summary>
    public interface IDisconnectedMessage : IConnectionChangedMessage
    {
        /// <summary>
        /// Exception that caused the disconnection.
        /// </summary>
        Exception? Exception { get; }

        /// <summary>
        /// The cancellation token that can be used to cancel the reconnection attempt.
        /// </summary>
        CancellationToken Token { get; }
    }
}

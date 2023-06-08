namespace RUCore.Common.Models.General
{
    /// <summary>
    /// Default implementation of <see cref="IDisconnectedMessage"/>.
    /// </summary>
    public class DisconnectedMessage : ConnectionChangedMessage, IDisconnectedMessage
    {
        /// <summary>
        /// Exception that caused the disconnect, if any.
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// The cancellation token that was used to cancel the connection.
        /// </summary>
        public CancellationToken Token { get; set; }
    }
}

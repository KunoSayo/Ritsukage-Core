namespace RUCore.Common.Enums
{
    /// <summary>
    /// Enum for the reason of connection
    /// </summary>
    public enum ConnectReason
    {
        /// <summary>
        /// User initiated the connection
        /// </summary>
        UserInitiated,

        /// <summary>
        /// Plugin initiated the connection
        /// </summary>
        PluginTriggered,

        /// <summary>
        /// Error encountered while connecting
        /// </summary>
        ErrorEncountered,

        /// <summary>
        /// Other reason
        /// </summary>
        Others
    }
}

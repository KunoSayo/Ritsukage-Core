using RUCore.Common.Enums;

namespace RUCore.Common.Models.General
{
    /// <summary>
    /// Connected message
    /// </summary>
    public class ConnectedMessage : ConnectionChangedMessage, IConnectedMessage
    {
        /// <summary>
        /// Reason
        /// </summary>
        public ConnectReason Reason { get; set; }
    }
}

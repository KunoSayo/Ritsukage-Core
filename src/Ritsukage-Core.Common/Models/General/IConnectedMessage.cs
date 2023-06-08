using RUCore.Common.Enums;

namespace RUCore.Common.Models.General
{
    /// <summary>
    /// Connected message
    /// </summary>
    public interface IConnectedMessage : IConnectionChangedMessage
    {
        /// <summary>
        /// Reason
        /// </summary>
        ConnectReason Reason { get; }
    }
}

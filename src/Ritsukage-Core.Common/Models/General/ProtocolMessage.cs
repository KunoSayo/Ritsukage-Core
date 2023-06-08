using RUCore.Common.Invoking;

namespace RUCore.Common.Models.General
{
    /// <summary>
    /// Message with protocol
    /// </summary>
    public abstract class ProtocolMessage : Message, IProtocolMessage
    {

        /// <inheritdoc/>
        public virtual long Id { get; set; }

        /// <inheritdoc/>
        public virtual DateTime Time { get; set; }
    }
}

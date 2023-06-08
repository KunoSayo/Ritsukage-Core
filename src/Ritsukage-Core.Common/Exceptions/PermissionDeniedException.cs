namespace RUCore.Common.Exceptions
{
    /// <summary>
    /// Permission denied exception.
    /// </summary>
    public sealed class PermissionDeniedException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public PermissionDeniedException() : base("Permission denied.")
        {
        }

        /// <summary>
        /// Constructor with message.
        /// </summary>
        /// <param name="message"></param>
        public PermissionDeniedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor with message and inner exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public PermissionDeniedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

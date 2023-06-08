namespace RUCore.Common.Exceptions
{
    /// <summary>
    /// Duplicate operation exception.
    /// </summary>
    public sealed class DuplicateOperationException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DuplicateOperationException() : base("This operation has already been performed.")
        {
        }

        /// <summary>
        /// Constructor with message.
        /// </summary>
        /// <param name="message"></param>
        public DuplicateOperationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor with message and inner exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DuplicateOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

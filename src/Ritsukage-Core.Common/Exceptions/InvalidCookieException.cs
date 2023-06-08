namespace RUCore.Common.Exceptions
{
    /// <summary>
    /// InvalidCookieException
    /// </summary>
    public sealed class InvalidCookieException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public InvalidCookieException() : base("The given Cookie is invalid.")
        {
        }

        /// <summary>
        /// Constructor with message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidCookieException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor with message and inner exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InvalidCookieException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

using System.Text.Json;
using System.Text.Json.Nodes;

namespace RUCore.Common.Exceptions
{
    /// <summary>
    /// UnknownResponseException
    /// </summary>
    public sealed class UnknownResponseException : Exception
    {
        /// <summary>
        /// Response
        /// </summary>
        public string? Response { get; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UnknownResponseException()
        {
        }

        /// <summary>
        /// Constructor with message.
        /// </summary>
        /// <param name="root"></param>
        public UnknownResponseException(in JsonElement root) : this(root.GetRawText())
        {
        }

        /// <summary>
        /// Constructor with message.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="message"></param>
        public UnknownResponseException(in JsonElement root, string? message) : this(root.GetRawText(), message)
        {
        }

        /// <summary>
        /// Constructor with message and inner exception.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="innerException"></param>
        public UnknownResponseException(in JsonElement root, Exception? innerException) : this(
            root.GetRawText(), innerException)
        {
        }

        /// <summary>
        /// Constructor with message and inner exception.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UnknownResponseException(in JsonElement root, string? message, Exception? innerException) : this(
            root.GetRawText(), message, innerException)
        {
        }

        /// <summary>
        /// Constructor with message.
        /// </summary>
        /// <param name="node"></param>
        public UnknownResponseException(JsonNode node) : this(node.ToJsonString())
        {
        }

        /// <summary>
        /// Constructor with message.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="message"></param>
        public UnknownResponseException(JsonNode node, string? message) : this(node.ToJsonString(), message)
        {
        }

        /// <summary>
        /// Constructor with message and inner exception.
        /// </summary>
        /// <param name="response"></param>
        public UnknownResponseException(string response) : this(response, "Unknown response.")
        {
        }

        /// <summary>
        /// Constructor with message and inner exception.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public UnknownResponseException(string response, string? message) : base(message)
        {
            Response = response;
        }

        /// <summary>
        /// Constructor with message and inner exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UnknownResponseException(string message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor with message and inner exception.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UnknownResponseException(string response, string? message, Exception? innerException) : base(
            message, innerException)
        {
            Response = response;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString() + Environment.NewLine + Response;
        }
    }
}

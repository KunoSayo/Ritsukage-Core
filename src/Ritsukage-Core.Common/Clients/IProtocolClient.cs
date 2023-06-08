namespace RUCore.Common.Clients
{
    /// <summary>
    /// Protocol client interface
    /// </summary>
    public interface IProtocolClient : IMessageClient
    {
        /// <summary>
        /// Gets a value indicating whether the client is connected to the server
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Connects to the server
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task ConnectAsync(CancellationToken token = default);

        /// <summary>
        /// Disconnects from the server
        /// </summary>
        void Disconnect();
    }
}

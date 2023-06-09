using System.Net.WebSockets;

namespace RUCore.Common.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="WebSocket"/>.
    /// </summary>
    public static class WebSocketExtensions
    {
        /// <summary>
        /// Receive a message from the WebSocket fully.
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="buffer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="EndOfStreamException"></exception>
        public static async ValueTask ReceiveFullyAsync(this WebSocket ws, Memory<byte> buffer,
                                                        CancellationToken token = default)
        {
            while (true)
            {
                ValueWebSocketReceiveResult result = await ws.ReceiveAsync(buffer, token);
                if (result.Count == buffer.Length)
                {
                    return;
                }

                if (result.EndOfMessage)
                {
                    throw new EndOfStreamException();
                }

                buffer = buffer[result.Count..];
            }
        }

        /// <summary>
        /// Receive a message from the WebSocket fully.
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async ValueTask<byte[]> ReceiveFullyAsync(this WebSocket webSocket,
                                                                CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            byte[] buffer = new byte[1024];
            using MemoryStream ms = new(1024);
            ValueWebSocketReceiveResult result;
            do
            {
                result = await webSocket.ReceiveAsync(buffer.AsMemory(), token);
                ms.Write(buffer, 0, result.Count);
            } while (!result.EndOfMessage);

            ms.Write(buffer, 0, result.Count);
            return ms.ToArray();
        }
    }
}

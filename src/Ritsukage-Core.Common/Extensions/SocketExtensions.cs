using System.Net.Sockets;

namespace RUCore.Common.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Socket"/>.
    /// </summary>
    public static class SocketExtensions
    {
        /// <summary>
        /// Receive data from socket fully.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static ValueTask ReceiveFullyAsync(this Socket socket, byte[] buffer, CancellationToken token = default)
        {
            return socket.ReceiveFullyAsync(new Memory<byte>(buffer, 0, buffer.Length), token);
        }

        /// <summary>
        /// Receive data from socket fully.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ValueTask ReceiveFullyAsync(this Socket       socket, byte[] buffer, int offset, int size,
                                                  CancellationToken token = default)
        {
            if (offset + size > buffer.Length)
                throw new ArgumentException(
                    "Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            Memory<byte> memory = new(buffer, offset, size);
            return socket.ReceiveFullyAsync(memory, token);
        }

        /// <summary>
        /// Receive data from socket fully.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="memory"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static ValueTask ReceiveFullyAsync(this Socket       socket, Memory<byte> memory,
                                                  CancellationToken token = default)
        {
#pragma warning disable CA2012 // Use ValueTasks correctly
            ValueTask<int> vt = socket.ReceiveAsync(memory, SocketFlags.None, token);
#pragma warning restore CA2012 // Use ValueTasks correctly
            if (vt.IsCompletedSuccessfully)
            {
                int recved = vt.Result;
                if (recved == memory.Length)
                    return default;
                vt = new ValueTask<int>(recved);
            }

            return Await(socket, memory, vt, token);
        }

        private static async ValueTask Await(Socket            socket, Memory<byte> memory, ValueTask<int> recvTask,
                                             CancellationToken token)
        {
            while (true)
            {
                int n = await recvTask;
                if (n < 1)
                    throw new SocketException((int)SocketError.ConnectionReset);
                else if (n == memory.Length)
                    return;
                memory   = memory[n..];
                recvTask = socket.ReceiveAsync(memory, SocketFlags.None, token);
            }
        }
    }
}

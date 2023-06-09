namespace RUCore.Common.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Stream"/>.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads the specified number of bytes from the stream, unless the end of the stream is reached first.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static ValueTask ReadFullyAsync(this Stream stream, byte[] buffer, CancellationToken token = default)
        {
            return stream.ReadFullyAsync(buffer, 0, buffer.Length, token);
        }

        /// <summary>
        /// Reads the specified number of bytes from the stream, unless the end of the stream is reached first.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="EndOfStreamException"></exception>
        public static async ValueTask ReadFullyAsync(this Stream stream, byte[] buffer, int offset, int size,
                                                     CancellationToken token = default)
        {
            if (offset + size > buffer.Length)
                throw new ArgumentException(
                    "Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            while (size > 0)
            {
                int n = await stream.ReadAsync(new Memory<byte>(buffer, offset, size), token);
                if (n < 1) throw new EndOfStreamException();
                offset += n;
                size   -= n;
            }
        }
    }
}

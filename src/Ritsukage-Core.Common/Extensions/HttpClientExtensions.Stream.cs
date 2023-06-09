namespace RUCore.Common.Extensions
{
    public static partial class HttpClientExtensions
    {
        /// <summary>
        /// Get stream from response
        /// </summary>
        /// <param name="responseTask"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<Stream> GetStreamAsync(this Task<HttpResponseMessage> responseTask,
                                                        CancellationToken token = default)
        {
            HttpResponseMessage response = await responseTask.ConfigureAwait(false);
            HttpContent? c = response.Content;
            return c != null ? await c.ReadAsStreamAsync(token) : Stream.Null;
        }
    }
}

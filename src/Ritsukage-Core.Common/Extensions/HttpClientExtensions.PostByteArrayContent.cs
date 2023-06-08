namespace RUCore.Common.Extensions
{
    public static partial class HttpClientExtensions
    {
        /// <param name="client">The <see cref="HttpClient"/>.</param>
        /// <param name="uri">The url the request is sent to.</param>
        /// <param name="content"></param>
        /// <param name="token">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <inheritdoc cref="HttpClient.PostAsync(string, HttpContent, CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsync(this HttpClient   client, Uri uri, byte[] content,
                                                          CancellationToken token = default)
        {
            return client.PostAsync(uri, new ByteArrayContent(content), token);
        }

        /// <param name="client"></param>
        /// <param name="url">The url the request is sent to.</param>
        /// <param name="content"></param>
        /// <param name="token"></param>
        /// <inheritdoc cref="PostAsync(HttpClient, Uri, byte[], CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsync(this HttpClient   client, string url, byte[] content,
                                                          CancellationToken token = default)
        {
            return client.PostAsync(new Uri(url), content, token);
        }
    }
}

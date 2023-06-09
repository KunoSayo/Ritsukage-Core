using System.Text;

namespace RUCore.Common.Extensions
{
    public static partial class HttpClientExtensions
    {
        /// <param name="client">The <see cref="HttpClient"/>.</param>
        /// <param name="uri">The url the request is sent to.</param>
        /// <param name="content"></param>
        /// <param name="encoding">The encoding to be used while encoding the <paramref name="content"/> to <see cref="StringContent"/>. Defaults to <see cref="Encoding.UTF8"/>.</param>
        /// <param name="token">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <inheritdoc cref="HttpClient.PostAsync(Uri, HttpContent, CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsync(this HttpClient client, Uri uri, string content,
                                                          Encoding? encoding, CancellationToken token = default)
        {
            return client.PostAsync(uri, new StringContent(content, encoding ?? Encoding.UTF8), token);
        }

        /// <param name="client"></param>
        /// <param name="url">The url the request is sent to.</param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        /// <param name="token"></param>
        /// <inheritdoc cref="PostAsync(HttpClient, Uri, string, Encoding?, CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string url, string content,
                                                          Encoding? encoding, CancellationToken token = default)
        {
            return client.PostAsync(new Uri(url), content, encoding, token);
        }

        /// <inheritdoc cref="PostAsync(HttpClient, Uri, string, Encoding?, CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsync(this HttpClient client, Uri uri, string content,
                                                          CancellationToken token = default)
        {
            return client.PostAsync(uri, content, null, token);
        }

        /// <inheritdoc cref="PostAsync(HttpClient, string, string, Encoding?, CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string url, string content,
                                                          CancellationToken token = default)
        {
            return client.PostAsync(url, content, null, token);
        }
    }
}

namespace RUCore.Common.Extensions
{
    public static partial class HttpClientExtensions
    {
        /// <inheritdoc cref="PostAsync(HttpClient, Uri, byte[], CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsync(this HttpClient   client, Uri uri,
                                                          CancellationToken token = default)
        {
            return client.PostAsync(uri, null!, token);
        }

        /// <inheritdoc cref="PostAsync(HttpClient, string, byte[], CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsync(this HttpClient   client, string url,
                                                          CancellationToken token = default)
        {
            return client.PostAsync(new Uri(url), token);
        }
    }
}

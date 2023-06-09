using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace RUCore.Common.Extensions
{
    /// <summary>
    /// Contains the extensions methods for easily performing request or handling response in HttpClient.
    /// </summary>
    public static partial class HttpClientExtensions
    {
        private static Version DefaultHttpVersion { get; } = new Version(2, 0);

        private static readonly MediaTypeHeaderValue DefaultJsonMediaType = new("application/json")
            { CharSet = "utf-8" };

        private static readonly MediaTypeHeaderValue DefaultPlainType = new("text/plain") { CharSet = "utf-8" };

        /// <param name="client">The <see cref="HttpClient"/>.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="uri">The Uri to request.</param>
        /// <param name="content">The contents of the HTTP message.</param>
        /// <param name="token">A <see cref="CancellationToken"/> which may be used to cancel the request operation.</param>
        /// <inheritdoc cref="HttpClient.SendAsync(HttpRequestMessage, CancellationToken)"/>
        public static async Task<HttpResponseMessage> SendAsync(this HttpClient client, HttpMethod method, Uri uri,
                                                                HttpContent? content, CancellationToken token = default)
        {
            using HttpRequestMessage request = new(method, uri)
            {
                Content = content,
                Version = DefaultHttpVersion
            };
            return await client.SendAsync(request, token);
        }

        /// <param name="responseTask">An asynchronous operation that represents the HTTP response.</param>
        /// <param name="token">A <see cref="CancellationToken"/> which may be used to cancel the serialize operation.</param>
        /// <inheritdoc cref="HttpContent.ReadAsByteArrayAsync()"/>
        public static async Task<byte[]> GetBytesAsync(this Task<HttpResponseMessage> responseTask,
                                                       CancellationToken token = default)
        {
            using HttpResponseMessage response = await responseTask.ConfigureAwait(false);
            return await response.Content.ReadAsByteArrayAsync(token);
        }

        /// <param name="responseTask">An asynchronous operation that represents the HTTP response.</param>
        /// <param name="token">A <see cref="CancellationToken"/> which may be used to cancel the serialize operation.</param>
        /// <inheritdoc cref="HttpContent.ReadAsStringAsync()"/>
        public static async Task<string> GetStringAsync(this Task<HttpResponseMessage> responseTask,
                                                        CancellationToken token = default)
        {
            using HttpResponseMessage response = await responseTask.ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync(token);
        }

        private static Encoding? GetEncoding(string? charset)
        {
            Encoding? encoding = null;

            if (charset != null)
            {
                try
                {
                    // Remove at most a single set of quotes.
                    encoding = charset.Length > 2 && charset[0] == '\"' && charset[^1] == '\"'
                        ? Encoding.GetEncoding(charset[1..^1])
                        : Encoding.GetEncoding(charset);
                }
                catch (ArgumentException e)
                {
                    throw new InvalidOperationException("The character set provided in ContentType is invalid.", e);
                }
            }

            return encoding;
        }

        /// <inheritdoc cref="GetObjectAsync{T}(Task{HttpResponseMessage}, JsonSerializerOptions?, CancellationToken)"/>
        public static Task<T?> GetObjectAsync<T>(this Task<HttpResponseMessage> responseTask,
                                                 CancellationToken token = default)
        {
            return responseTask.GetObjectAsync<T?>(null, token);
        }

        /// <summary>
        /// Deserializes the HTTP content to an instance of <typeparamref name="T"/> as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseTask">An asynchronous operation that represents the HTTP response.</param>
        /// <param name="options">A <see cref="JsonSerializerOptions"/> to be used while deserializing the HTTP content.</param>
        /// <param name="token">A <see cref="CancellationToken"/> which may be used to cancel the deserialize operation.</param>
        /// <returns>A task that represents the asynchronous deserialize operation.</returns>
        public static async Task<T?> GetObjectAsync<T>(this Task<HttpResponseMessage> responseTask,
                                                       JsonSerializerOptions? options,
                                                       CancellationToken token = default)
        {
            using HttpResponseMessage response = await responseTask.ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<T?>(options, token);
        }

        /// <inheritdoc cref="GetObjectAsync(Task{HttpResponseMessage}, Type, JsonSerializerOptions?, CancellationToken)"/>
        public static Task<object?> GetObjectAsync(this Task<HttpResponseMessage> responseTask, Type returnType,
                                                   CancellationToken token = default)
        {
            return responseTask.GetObjectAsync(returnType, null, token);
        }

        /// <summary>
        /// Deserializes the HTTP content to an instance of <paramref name="returnType"/> as an asynchronous operation.
        /// </summary>
        /// <param name="responseTask"></param>
        /// <param name="returnType">The type of the HTTP content to convert to and return.</param>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <inheritdoc cref="GetObjectAsync{T}(Task{HttpResponseMessage}, JsonSerializerOptions?, CancellationToken)"/>
        public static async Task<object?> GetObjectAsync(this Task<HttpResponseMessage> responseTask, Type returnType,
                                                         JsonSerializerOptions? options,
                                                         CancellationToken token = default)
        {
            using HttpResponseMessage response = await responseTask.ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync(returnType, options, token);
        }

        /// <inheritdoc cref="GetJsonAsync(Task{HttpResponseMessage}, JsonDocumentOptions, CancellationToken)"/>
        public static Task<JsonDocument> GetJsonAsync(this Task<HttpResponseMessage> responseTask,
                                                      CancellationToken token = default)
        {
            return responseTask.GetJsonAsync(default, token);
        }

        /// <summary>
        /// Deserializes the HTTP content to an instance of <see cref="JsonDocument"/> as an asynchronous operation.
        /// </summary>
        /// <param name="responseTask">An asynchronous operation that represents the HTTP response.</param>
        /// <param name="options">A <see cref="JsonSerializerOptions"/> to be used while deserializing the HTTP content.</param>
        /// <param name="token">A <see cref="CancellationToken"/> which may be used to cancel the deserialize operation.</param>
        /// <returns>A task that represents the asynchronous deserialize operation.</returns>
        public static async Task<JsonDocument> GetJsonAsync(this Task<HttpResponseMessage> responseTask,
                                                            JsonDocumentOptions options,
                                                            CancellationToken token = default)
        {
            using HttpResponseMessage response = await responseTask.ConfigureAwait(false);
            Stream
                stream = response.Content
                                 .ReadAsStream(token); // Since Content.ReadAsStreamAsync is returned synchronously.
            Encoding? encoding = GetEncoding(response.Content.Headers.ContentType?.CharSet);
            if (encoding != null && encoding != Encoding.UTF8)
            {
                stream = Encoding.CreateTranscodingStream(stream, encoding, Encoding.UTF8);
            }

            using (stream)
            {
                return await JsonDocument.ParseAsync(stream, options, token);
            }
        }

        /// <summary>
        /// Sets Content-Type in response to application/json.
        /// </summary>
        /// <param name="responseTask">An asynchronous operation that represents the HTTP response.</param>
        public static Task<HttpResponseMessage> ForceJson(this Task<HttpResponseMessage> responseTask)
        {
            return responseTask.ContinueWith(p =>
            {
                if (p.IsCompletedSuccessfully)
                {
                    HttpContentHeaders headers = p.Result.Content.Headers;
                    if (headers.ContentType == null)
                    {
                        headers.ContentType = DefaultJsonMediaType;
                    }
                    else if (headers.ContentType.MediaType != "application/json")
                    {
                        headers.ContentType.MediaType = "application/json";
                    }
                }

                return p;
            }, TaskContinuationOptions.ExecuteSynchronously).Unwrap();
        }

        /// <summary>
        /// Sets Content-Type in response to text/plain; charset=utf-8.
        /// </summary>
        /// <param name="responseTask">An asynchronous operation that represents the HTTP response.</param>
        public static Task<HttpResponseMessage> ForcePlain(this Task<HttpResponseMessage> responseTask)
        {
            return responseTask.ContinueWith(p =>
            {
                if (p.IsCompletedSuccessfully)
                {
                    HttpContentHeaders headers = p.Result.Content.Headers;
                    headers.ContentType = DefaultPlainType;
                }

                return p;
            }, TaskContinuationOptions.ExecuteSynchronously).Unwrap();
        }

        /// <summary>
        /// Ensures that the HTTP response is successful.
        /// </summary>
        /// <param name="responseTask"></param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> EnsureSuccessStatusCode(this Task<HttpResponseMessage> responseTask)
        {
            return responseTask.ContinueWith(p =>
            {
                if (p.IsCompletedSuccessfully)
                {
                    p.Result.EnsureSuccessStatusCode();
                }

                return p;
            }, TaskContinuationOptions.ExecuteSynchronously).Unwrap();
        }

        /// <summary>
        /// Sets User-Agent in request.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="userAgent"></param>
        public static void SetUserAgent(this HttpClient client, string userAgent)
        {
            client.DefaultRequestHeaders.SetUserAgent(userAgent);
        }

        /// <summary>
        /// Sets Accept in request.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="accept"></param>
        public static void SetAccept(this HttpClient client, string accept)
        {
            client.DefaultRequestHeaders.SetAccept(accept);
        }

        /// <summary>
        /// Sets Accept-Language in request.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="acceptLanguage"></param>
        public static void SetAcceptLanguage(this HttpClient client, string acceptLanguage)
        {
            client.DefaultRequestHeaders.SetAcceptLanguage(acceptLanguage);
        }

        /// <summary>
        /// Sets Accept-Encoding in request.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="mode"></param>
        /// <param name="site"></param>
        /// <param name="dest"></param>
        public static void SetSecPolicy(this HttpClient client, string? mode = "cors", string? site = "same-site",
                                        string? dest = "empty")
        {
            client.DefaultRequestHeaders.SetSecPolicy(mode, site, dest);
        }
    }
}

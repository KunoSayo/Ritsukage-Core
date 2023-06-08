using System.Net.Http.Headers;

namespace RUCore.Common.Extensions
{
    /// <summary>
    /// Extension for HttpRequestHeaders
    /// </summary>
    public static class HttpRequestHeadersExtension
    {
        /// <summary>
        /// Set User-Agent
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="userAgent"></param>
        public static void SetUserAgent(this HttpRequestHeaders headers, string userAgent)
        {
            HttpHeaderValueCollection<ProductInfoHeaderValue> ua = headers.UserAgent;
            ua.Clear();
            ua.ParseAdd(userAgent);
        }

        /// <summary>
        /// Set Accept
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="accept"></param>
        public static void SetAccept(this HttpRequestHeaders headers, string accept)
        {
            HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> a = headers.Accept;
            a.Clear();
            a.ParseAdd(accept);
        }

        /// <summary>
        /// Set Accept-Language
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="acceptLanguage"></param>
        public static void SetAcceptLanguage(this HttpRequestHeaders headers, string acceptLanguage)
        {
            HttpHeaderValueCollection<StringWithQualityHeaderValue> al = headers.AcceptLanguage;
            al.Clear();
            al.ParseAdd(acceptLanguage);
        }

        /// <summary>
        /// Set Sec-Fetch-Mode, Sec-Fetch-Site, Sec-Fetch-Dest
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="mode"></param>
        /// <param name="site"></param>
        /// <param name="dest"></param>
        public static void SetSecPolicy(this HttpRequestHeaders headers,            string? mode = "cors",
                                        string?                 site = "same-site", string? dest = "empty")
        {
            if (!string.IsNullOrEmpty(mode))
                headers.Add("Sec-Fetch-Mode", mode);
            if (!string.IsNullOrEmpty(site))
                headers.Add("Sec-Fetch-Site", site);
            if (!string.IsNullOrEmpty(dest))
                headers.Add("Sec-Fetch-Dest", dest);
        }
    }
}

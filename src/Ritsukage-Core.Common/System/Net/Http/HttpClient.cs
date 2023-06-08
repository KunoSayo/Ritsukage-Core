using System.Net;
using BaseHttpClient = System.Net.Http.HttpClient;

namespace RUCore.Common.System.Net.Http
{
    /// <summary>
    /// HttpClient
    /// </summary>
    public abstract class HttpClient : BaseHttpClient
    {
        /// <summary>
        /// Handler
        /// </summary>
        protected readonly HttpMessageHandler _handler;

        /// <summary>
        /// Cookie
        /// </summary>
        public CookieContainer Cookie
        {
            get => _handler switch
            {
                HttpClientHandler clientHandler   => clientHandler.CookieContainer,
                SocketsHttpHandler socketsHandler => socketsHandler.CookieContainer,
                _                                 => throw new NotSupportedException()
            };
            set => _ = _handler switch
            {
                HttpClientHandler clientHandler   => clientHandler.CookieContainer = value,
                SocketsHttpHandler socketsHandler => socketsHandler.CookieContainer = value,
                _                                 => throw new NotSupportedException()
            };
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public HttpClient() : this(new HttpClientHandler())
        {
        }

        /// <summary>
        /// Constructor with handler.
        /// </summary>
        /// <param name="handler"></param>
        public HttpClient(HttpMessageHandler handler) : base(handler)
        {
            _handler = handler;
        }

        /// <summary>
        /// ClearCookie
        /// </summary>
        public void ClearCookie()
        {
            Cookie = new CookieContainer();
        }

        /// <summary>
        /// SetCookie
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="cookie"></param>
        public void SetCookie(Uri uri, string cookie)
        {
            Cookie.SetCookies(uri, cookie);
        }
    }
}

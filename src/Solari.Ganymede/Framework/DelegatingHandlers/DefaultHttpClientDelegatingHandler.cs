using System.Net;
using System.Net.Http;

namespace Solari.Ganymede.Framework.DelegatingHandlers
{
    public class DefaultHttpClientDelegatingHandler : HttpClientHandler
    {
        public DefaultHttpClientDelegatingHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Solari.Ganymede.Builders;
using Solari.Ganymede.Domain.Options;
using Solari.Ganymede.Framework;

namespace Solari.Ganymede.Extensions
{
    internal static class HttpClientExtensions
    {
        public static void SetBaseAddress(this HttpClient httpClient, string baseAddress)
        {
            if (string.IsNullOrEmpty(baseAddress)) return;

            httpClient.BaseAddress = new Uri(baseAddress);
        }

        public static void SetDefaultRequestHeaders(this HttpClient httpClient, IEnumerable<GanymedeRequestHeader> headerOptions)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

            IEnumerable<GanymedeRequestHeader> options = headerOptions as GanymedeRequestHeader[] ?? headerOptions.ToArray();
            if (!options.Any()) return;

            var headerBuilder = new HttpClientHeaderCommandBuilder(httpClient);

            foreach (GanymedeRequestHeader header in options)
            {
                IHeaderBuilderCommand headerCommand = CommandTypeRegistry.Instance.TryGetCommandType(header.Name.Trim().ToUpperInvariant());
                headerCommand?.Execute(headerBuilder, header.KeyOrQuality, header.Value);
            }
        }

        public static void SetMaxResponseContentBufferSize(this HttpClient httpClient, long maxContentBufferSize)
        {
            if (maxContentBufferSize > 0)
                httpClient.MaxResponseContentBufferSize = maxContentBufferSize;
        }

        public static void SetTimeout(this HttpClient httpClient, TimeSpan timeout)
        {
            if (timeout > TimeSpan.Zero)
                httpClient.Timeout = timeout;
        }
    }
}
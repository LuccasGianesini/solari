using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Solari.Deimos.Abstractions;

namespace Solari.Deimos.CorrelationId
{
    public static class DeimosCorrelationIdExtensions
    {
        /// <summary>
        ///     Add CorrelationId header with default trace identifier generator.
        /// </summary>
        /// <param name="requestMessage">Request message</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static HttpRequestMessage AddCorrelationIdHeader(this HttpRequestMessage requestMessage)
        {
            if (requestMessage == null) throw new ArgumentNullException(nameof(requestMessage));

            requestMessage.Headers.Add(DeimosConstants.DefaultCorrelationIdHeader, TraceIdGenerator.Create());

            return requestMessage;
        }

        /// <summary>
        ///     Add CorrelationId header.
        /// </summary>
        /// <param name="requestMessage">Request message</param>
        /// <param name="value">CorrelationId value</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static HttpRequestMessage AddCorrelationIdHeader(this HttpRequestMessage requestMessage, string value)
        {
            if (requestMessage == null) throw new ArgumentNullException(nameof(requestMessage));

            requestMessage.Headers.Add(DeimosConstants.DefaultCorrelationIdHeader, value);

            return requestMessage;
        }

        /// <summary>
        ///     Checks if the request message contains the CorrelationId header and if the value is valid.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ContainsCorrelationIdHeader(this HttpRequestMessage requestMessage)
        {
            if (requestMessage == null) throw new ArgumentNullException(nameof(requestMessage));

            bool exists = requestMessage.Headers.TryGetValues(DeimosConstants.DefaultCorrelationIdHeader, out IEnumerable<string> values);

            return exists && !string.IsNullOrEmpty(values.FirstOrDefault());
        }

        /// <summary>
        ///     Gets the request message CorrelationId header value.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetCorrelationIdHeaderValue(this HttpRequestMessage requestMessage)
        {
            if (requestMessage == null) throw new ArgumentNullException(nameof(requestMessage));

            return requestMessage.Headers.TryGetValues(DeimosConstants.DefaultCorrelationIdHeader, out IEnumerable<string> values)
                       ? values.FirstOrDefault()
                       : string.Empty;
        }
    }
}
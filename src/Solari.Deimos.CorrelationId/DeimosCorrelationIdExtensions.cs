using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Solari.Deimos.Abstractions;
using Solari.Sol;

namespace Solari.Deimos.CorrelationId
{
    public static class DeimosCorrelationIdExtensions
    {
        public static bool TryExtractCorrelationContext(this HttpRequest request, out ICorrelationContext correlationContext)
        {
            if (request == null)
            {
                correlationContext = null;
                return false;
            }

            correlationContext = new DefaultCorrelationContext
            {
                MessageId = request.Headers.GetHeaderValue(DeimosConstants.MessageIdHeader),
                EnvoyCorrelationContext = new DefaultEnvoyCorrelationContextContext
                {
                    Flags = request.Headers.GetHeaderValue(DeimosConstants.EnvoyFlagsHeader),
                    Sampled = request.Headers.GetHeaderValue(DeimosConstants.EnvoySampledHeader),
                    RequestId = request.Headers.GetHeaderValue(DeimosConstants.RequestIdHeader),
                    SpanId = request.Headers.GetHeaderValue(DeimosConstants.EnvoySpanIdHeader),
                    TraceId = request.Headers.GetHeaderValue(DeimosConstants.EnvoyTraceIdHeader),
                    OtSpanContext = request.Headers.GetHeaderValue(DeimosConstants.EnvoyOutgoingSpanContext),
                    ParentSpanId = request.Headers.GetHeaderValue(DeimosConstants.EnvoyParentSpanIdHeader)
                }
            };

            return true;
        }

        public static string GetHeaderValue(this IHeaderDictionary dictionary, string headerKey)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (string.IsNullOrEmpty(headerKey)) throw new ArgumentException("Header key cannot be null or empty", nameof(headerKey));

            if (!dictionary.ContainsKey(headerKey))
                return string.Empty;
            return dictionary.TryGetValue(headerKey, out StringValues value) ? value.FirstOrDefault() : string.Empty;
        }

        public static HttpRequestMessage AddCorrelationContext(this HttpRequestMessage message, ICorrelationContext context)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (context == null) throw new ArgumentNullException(nameof(context), "Cannot add a null correlation context");

            if (context.EnvoyCorrelationContext == null)
                throw new ArgumentNullException(nameof(context.EnvoyCorrelationContext),
                                                "The envoy context is used to provide tracing in kubernetes. Please provide one ");
            if (!string.IsNullOrEmpty(context.MessageId))
                message.Headers.Add(context.MessageIdHeader, context.MessageId);
            IEnvoyCorrelationContext envoy = context.EnvoyCorrelationContext;

            message.Headers.Add(envoy.FlagsHeader, envoy.Flags);
            message.Headers.Add(envoy.SampledHeader, envoy.Sampled);
            message.Headers.Add(envoy.RequestIdHeader, envoy.RequestId);
            message.Headers.Add(envoy.SpanIdHeader, envoy.SpanId);
            message.Headers.Add(envoy.ParentSpanIdHeader, envoy.ParentSpanId);
            message.Headers.Add(envoy.OtSpanContextHeader, envoy.OtSpanContext);
            return message;
        }

        public static bool TryExtractCorrelationContext(this HttpRequestMessage message, out ICorrelationContext correlationContext)
        {
            if (message == null)
            {
                correlationContext = null;
                return false;
            }

            correlationContext = new DefaultCorrelationContext
            {
                MessageId = message.GetHeaderValue(DeimosConstants.MessageIdHeader),
                EnvoyCorrelationContext = new DefaultEnvoyCorrelationContextContext
                {
                    Flags = message.GetHeaderValue(DeimosConstants.EnvoyFlagsHeader),
                    Sampled = message.GetHeaderValue(DeimosConstants.EnvoySampledHeader),
                    RequestId = message.GetHeaderValue(DeimosConstants.RequestIdHeader),
                    SpanId = message.GetHeaderValue(DeimosConstants.EnvoySpanIdHeader),
                    TraceId = message.GetHeaderValue(DeimosConstants.EnvoyTraceIdHeader),
                    OtSpanContext = message.GetHeaderValue(DeimosConstants.EnvoyOutgoingSpanContext),
                    ParentSpanId = message.GetHeaderValue(DeimosConstants.EnvoyParentSpanIdHeader)
                }
            };
            return true;
        }

        /// <summary>
        ///     Add CorrelationId header with default trace identifier generator.
        /// </summary>
        /// <param name="requestMessage">Request message</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static HttpRequestMessage AddCorrelationIdHeader(this HttpRequestMessage requestMessage)
        {
            if (requestMessage == null) throw new ArgumentNullException(nameof(requestMessage));

            requestMessage.Headers.Add(DeimosConstants.RequestIdHeader, TraceIdGenerator.Create());

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

            requestMessage.Headers.Add(DeimosConstants.RequestIdHeader, value);

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

            bool exists = requestMessage.Headers.TryGetValues(DeimosConstants.RequestIdHeader, out IEnumerable<string> values);

            return exists && !string.IsNullOrEmpty(values.FirstOrDefault());
        }

        /// <summary>
        ///     Gets the request message CorrelationId header value.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="headerKey">The key of the header being extracted</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetHeaderValue(this HttpRequestMessage requestMessage, string headerKey)
        {
            if (requestMessage == null) throw new ArgumentNullException(nameof(requestMessage));

            return requestMessage.Headers.TryGetValues(headerKey, out IEnumerable<string> values)
                       ? values.FirstOrDefault()
                       : string.Empty;
        }
    }
}
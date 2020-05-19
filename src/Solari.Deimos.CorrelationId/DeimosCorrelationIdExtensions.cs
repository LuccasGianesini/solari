using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Solari.Deimos.Abstractions;

namespace Solari.Deimos.CorrelationId
{
    public static class DeimosCorrelationIdExtensions
    {
        public static HttpResponse AddCorrelationContext(this HttpResponse response, IServiceProvider provider)
        {
            var manager = provider.GetService<ICorrelationContextManager>();
            ICorrelationContext current = manager.GetOrCreateAndSet();
            if (current?.EnvoyCorrelationContext == null) return response;
            response.Headers.Add(current.EnvoyCorrelationContext.FlagsHeader, current.EnvoyCorrelationContext.Flags);
            response.Headers.Add(current.EnvoyCorrelationContext.SampledHeader, current.EnvoyCorrelationContext.Sampled);
            response.Headers.Add(current.EnvoyCorrelationContext.RequestIdHeader, current.EnvoyCorrelationContext.RequestId);
            response.Headers.Add(current.EnvoyCorrelationContext.SpanIdHeader, current.EnvoyCorrelationContext.SpanId);
            response.Headers.Add(current.EnvoyCorrelationContext.ParentSpanIdHeader, current.EnvoyCorrelationContext.ParentSpanId);
            response.Headers.Add(current.EnvoyCorrelationContext.TraceIdHeader, current.EnvoyCorrelationContext.TraceId);
            response.Headers.Add(current.EnvoyCorrelationContext.OtSpanContextHeader, current.EnvoyCorrelationContext.OtSpanContext);
            return response;
        }

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
            if (!string.IsNullOrEmpty(context.MessageId))
                message.Headers.Add(context.MessageIdHeader, context.MessageId);
            IEnvoyCorrelationContext envoy = context.EnvoyCorrelationContext;

            message.Headers.Add(envoy.FlagsHeader, envoy.Flags);
            message.Headers.Add(envoy.SampledHeader, envoy.Sampled);
            message.Headers.Add(envoy.RequestIdHeader, envoy.RequestId);
            message.Headers.Add(envoy.SpanIdHeader, envoy.SpanId);
            message.Headers.Add(envoy.ParentSpanIdHeader, envoy.ParentSpanId);
            message.Headers.Add(envoy.TraceIdHeader, envoy.TraceId);
            message.Headers.Add(envoy.OtSpanContextHeader, envoy.OtSpanContext);
            return message;
        }

        public static bool TryExtractCorrelationContext(this HttpResponseMessage message, out ICorrelationContext correlationContext)
        {
            if (message == null)
            {
                correlationContext = null;
                return false;
            }

            correlationContext = new DefaultCorrelationContext
            {
                MessageId = message.Headers.GetValues(DeimosConstants.MessageIdHeader).FirstOrDefault(),
                EnvoyCorrelationContext = new DefaultEnvoyCorrelationContextContext
                {
                    Flags = message.Headers.GetValues(DeimosConstants.EnvoyFlagsHeader).FirstOrDefault(),
                    Sampled = message.Headers.GetValues(DeimosConstants.EnvoySampledHeader).FirstOrDefault(),
                    RequestId = message.Headers.GetValues(DeimosConstants.RequestIdHeader).FirstOrDefault(),
                    SpanId = message.Headers.GetValues(DeimosConstants.EnvoySpanIdHeader).FirstOrDefault(),
                    TraceId = message.Headers.GetValues(DeimosConstants.EnvoyTraceIdHeader).FirstOrDefault(),
                    OtSpanContext = message.Headers.GetValues(DeimosConstants.EnvoyOutgoingSpanContext).FirstOrDefault(),
                    ParentSpanId = message.Headers.GetValues(DeimosConstants.EnvoyParentSpanIdHeader).FirstOrDefault()
                }
            };
            return true;
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
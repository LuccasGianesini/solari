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
        public static HttpResponse AddCorrelationContext(this HttpResponse response,
                                                         IServiceProvider provider)
        {
            var handler = provider.GetService<ICorrelationContextAccessor>();
            var factory = provider.GetService<ICorrelationContextHandler>();
            if (handler == null || factory == null) throw new ArgumentException("CorrelationContext or CorrelationContextFactory has a null value. FIX IT!!!!");
            ICorrelationContext context = handler.Current;
            if (context == null) context = factory.Create_Root_From_System_Diagnostics_Activity_And_Tracers();
            ICorrelationContext updated = factory.UpdateCurrent(context);
            response.Headers.Add(updated.EnvoyCorrelationContext.FlagsHeader, updated.EnvoyCorrelationContext.Flags);
            response.Headers.Add(updated.EnvoyCorrelationContext.SampledHeader, updated.EnvoyCorrelationContext.Sampled);
            response.Headers.Add(updated.EnvoyCorrelationContext.RequestIdHeader, updated.EnvoyCorrelationContext.RequestId);
            response.Headers.Add(updated.EnvoyCorrelationContext.SpanIdHeader, updated.EnvoyCorrelationContext.SpanId);
            response.Headers.Add(updated.EnvoyCorrelationContext.ParentSpanIdHeader, updated.EnvoyCorrelationContext.ParentSpanId);
            response.Headers.Add(updated.EnvoyCorrelationContext.TraceIdHeader, updated.EnvoyCorrelationContext.TraceId);
            response.Headers.Add(updated.EnvoyCorrelationContext.OtSpanContextHeader, updated.EnvoyCorrelationContext.OtSpanContext);
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

            if (context.EnvoyCorrelationContext == null)
                throw new ArgumentNullException(nameof(context.EnvoyCorrelationContext),
                                                "The envoy context is used to provide tracing in kubernetes. Please provide one");
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
using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTracing;
using Solari.Deimos.Abstractions;
using ITracer = OpenTracing.ITracer;

namespace Solari.Deimos.CorrelationId.Framework
{
    public class CorrelationContextFactory : ICorrelationContextFactory
    {
        private readonly IServiceProvider _provider;

        public CorrelationContextFactory(IServiceProvider provider) { _provider = provider; }

        public ICorrelationContext Create(IEnvoyCorrelationContext envoyCorrelationContext)
        {
            if (envoyCorrelationContext == null) throw new ArgumentNullException(nameof(envoyCorrelationContext));
            return new DefaultCorrelationContext
            {
                EnvoyCorrelationContext = envoyCorrelationContext
            };
        }

        public ICorrelationContext Create(string messageId, IEnvoyCorrelationContext envoyCorrelationContext)
        {
            return new DefaultCorrelationContext
            {
                MessageId = messageId,
                EnvoyCorrelationContext = envoyCorrelationContext
            };
        }


        public ICorrelationContext CreateFromJaegerTracer(string messageId = "")
        {
            var tracer = _provider.GetService<ITracer>();

            if (tracer.ActiveSpan == null)
            {
                return new DefaultCorrelationContext();
            }

            if (tracer.ActiveSpan.Context == null)
            {
                return new DefaultCorrelationContext();
            }

            var ctx = tracer.ActiveSpan.Context.ToString();
            IEnvoyCorrelationContext envoy = ExtractFromJaegerContext(ctx);
            envoy.OtSpanContext = ctx;
            envoy.RequestId = Activity.Current == null ? new TraceIdGenerator().TraceIdentifier : Activity.Current?.RootId;

            return Create(messageId, envoy);
        }


        public IEnvoyCorrelationContext CreateEnvoy(string traceId, string spanId,
                                                    string requestId = null, string parentSpanId = null,
                                                    string sampled = "1", string flags = null,
                                                    string outgoingSpanContext = null)
        {
            return new DefaultEnvoyCorrelationContextContext
            {
                Sampled = sampled,
                RequestId = requestId,
                SpanId = spanId,
                Flags = flags,
                OtSpanContext = outgoingSpanContext,
                TraceId = traceId,
                ParentSpanId = parentSpanId
            };
        }

        private static IEnvoyCorrelationContext ExtractFromJaegerContext(string spanContext)
        {
            string[] ctx = spanContext.Split(":");
            return new DefaultEnvoyCorrelationContextContext
            {
                Sampled = ctx[3],
                SpanId = ctx[1],
                TraceId = ctx[0],
                ParentSpanId = ctx[2].Equals("0") ? "" : ctx[2]
            };
        }
    }
}
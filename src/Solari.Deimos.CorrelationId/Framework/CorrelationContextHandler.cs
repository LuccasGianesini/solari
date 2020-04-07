using System;
using System.Diagnostics;
using Elastic.Apm;
using Elastic.Apm.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTracing;
using Solari.Deimos.Abstractions;
using ISpan = Elastic.Apm.Api.ISpan;
using ITracer = OpenTracing.ITracer;

namespace Solari.Deimos.CorrelationId.Framework
{
    public class CorrelationContextHandler : ICorrelationContextHandler
    {
        private readonly IServiceProvider _provider;

        public CorrelationContextHandler(IServiceProvider provider) { _provider = provider; }

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
        public ICorrelationContext Create_Root_From_System_Diagnostics_Activity_And_Tracers(string messageId = "")
        {
            DeimosOptions opt = _provider.GetService<IOptions<DeimosOptions>>().Value;
            Activity currentActivity = Activity.Current;

            if (!opt.UseJaeger) return Create(messageId, null);
            
            var tracer = _provider.GetService<ITracer>();
            ISpanContext spanCtx = tracer.ActiveSpan.Context;
            IEnvoyCorrelationContext envoy = ExtractFromJaegerContext(spanCtx.ToString());
            envoy.RequestId = currentActivity == null ? new TraceIdGenerator().TraceIdentifier : currentActivity?.RootId;

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

        public ICorrelationContext UpdateCurrent(ICorrelationContext current)
        {
            DeimosOptions opt = _provider.GetService<IOptions<DeimosOptions>>().Value;
            if (!opt.UseJaeger) return current;
            var tracer = _provider.GetService<ITracer>();
            ISpanContext spanCtx = tracer.ActiveSpan.Context;
            IEnvoyCorrelationContext envoy = ExtractFromJaegerContext(spanCtx.ToString());
            envoy.RequestId = current.EnvoyCorrelationContext.RequestId;
            envoy.OtSpanContext = spanCtx.ToString();
            current.EnvoyCorrelationContext = envoy;
            return current;
        }

        private static string ValueChooser(string one, string two) { return string.IsNullOrEmpty(one) ? two : one; }
    }
}
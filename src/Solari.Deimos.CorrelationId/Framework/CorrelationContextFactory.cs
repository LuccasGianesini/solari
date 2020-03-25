using System;
using System.Collections.Generic;
using System.Diagnostics;
using Elastic.Apm;
using Elastic.Apm.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTracing;
using OpenTracing.Propagation;
using Solari.Deimos.Abstractions;
using ISpan = Elastic.Apm.Api.ISpan;
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
            return new DefaultCorrelationContext()
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


        public ICorrelationContext Create_Root_From_System_Diagnostics_Activity_And_Tracers(string messageId = "")
        {
            DeimosOptions opt = _provider.GetService<IOptions<DeimosOptions>>().Value;
            IEnvoyCorrelationContext envoy = null;
            Activity currentActivity = Activity.Current;

            if (opt.UseJaeger)
            {
                var tracer = _provider.GetService<ITracer>();
                ISpanContext spanCtx = tracer.ActiveSpan.Context;
                envoy = CreateEnvoy(ValueChooser(spanCtx.TraceId, currentActivity.TraceId.ToHexString()),
                                    ValueChooser(spanCtx.SpanId, currentActivity.SpanId.ToHexString()),
                                    currentActivity.Id, "");
            }

            if (!opt.UseElasticApm) return Create(messageId, envoy);
            
            ISpan currentSpan = Agent.Tracer.CurrentSpan;
            envoy = CreateEnvoy(ValueChooser(currentSpan.TraceId, currentActivity.TraceId.ToHexString()),
                                ValueChooser(currentSpan.Id, currentActivity.SpanId.ToHexString()),
                                currentActivity.Id, "");

            return Create(messageId, envoy);
        }

        
        private static string ValueChooser(string one, string two) { return string.IsNullOrEmpty(one) ? two : one; }

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
    }
}
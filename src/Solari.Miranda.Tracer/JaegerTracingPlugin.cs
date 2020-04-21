using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Jaeger;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing;
using OpenTracing.Tag;
using RawRabbit.Enrichers.GlobalExecutionId;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Pipe;
using RawRabbit.Pipe.Middleware;
using Serilog;
using Solari.Deimos.CorrelationId;
using Solari.Miranda.Abstractions;

namespace Solari.Miranda.Tracer
{
    public class JaegerTracingPlugin : StagedMiddleware
    {
        private readonly ITracer _tracer;
        private readonly ICorrelationContextManager _contextManager;
        private bool _canTrace;

        public JaegerTracingPlugin(IServiceProvider provider)
        {
            _tracer = provider.GetService<ITracer>();
            _contextManager = provider.GetService<ICorrelationContextManager>();
            if (_tracer != null || _contextManager != null)
            {
                _canTrace = true;
            }
        }

        public override async Task InvokeAsync(IPipeContext context, CancellationToken token = new CancellationToken())
        {
            if (!_canTrace)
            {
                await Next.InvokeAsync(context, token);
            }
            else
            {
                string messageName = context.GetMessage().GetType().Name;
                IMirandaMessageContext ctx = context.GetMessageContext() as IMirandaMessageContext;

                await Trace(context, token, messageName, ctx);
            }
        }

        private async Task Trace(IPipeContext context, CancellationToken token, string messageName, IMirandaMessageContext ctx)
        {
            using (IScope scope = BuildScope(messageName, ctx.EnvoyCorrelationContext.OtSpanContext))
            {
                Log.Debug($"Build scope with span context {ctx.EnvoyCorrelationContext.OtSpanContext}");
                ISpan span = scope.Span;
                span.Log($"Started processing: {messageName} [id: {ctx.MessageId}]");
                try
                {
                    await Next.InvokeAsync(context, token);
                }
                catch (Exception ex)
                {
                    span.SetTag(Tags.Error, true);
                    span.Log(ex.Message);
                }

                span.Log($"Finished processing: {messageName} [id: {ctx.MessageId}]");
            }
        }

        // private IMirandaMessageContext GetMirandaMessageContext(IPipeContext context)
        // {
        //     if (!(context.GetMessageContext() is IMirandaMessageContext ctx))
        //     {
        //         if (ctx == null)
        //         {
        //             ctx = new MirandaMessageContext
        //             {
        //                 Empty = true,
        //                 EnvoyCorrelationContext = new DefaultEnvoyCorrelationContextContext
        //                 {
        //                     OtSpanContext = ""
        //                 }
        //             };
        //         }
        //     }

        //     return ctx;
        // }

        private IScope BuildScope(string messageName, string serializedSpanContext)
        {
            var spanBuilder = _tracer
                              .BuildSpan($"processing-{messageName}")
                              .WithTag("message-type", messageName);

            if (string.IsNullOrEmpty(serializedSpanContext))
            {
                return spanBuilder.StartActive(true);
            }

            var spanContext = SpanContext.ContextFromString(serializedSpanContext);

            return spanBuilder
                   .AddReference(References.FollowsFrom, spanContext)
                   .StartActive(true);
        }

        public override string StageMarker { get; } = "JaegerTracerPlugin";
    }
}
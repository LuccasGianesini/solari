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
    public class JaegerTracingMiddleware : StagedMiddleware
    {
        private readonly ITracer _tracer;
        private readonly ICorrelationContextManager _contextManager;
        private bool _canTrace;
        public override string StageMarker { get; } = global::RawRabbit.Pipe.StageMarker.MessageDeserialized;

        public JaegerTracingMiddleware(IServiceProvider provider)
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
            string messageId = context.GetDeliveryEventArgs().BasicProperties.MessageId;
            if (!_canTrace || context.GetMessage() == null)
            {
                MirandaLogger.JaegerTracingPlugin.LogSkippingMessage(messageId);
                if (context == null)
                {
                    
                }
                await Next.InvokeAsync(context, token);
            }
            else
            {
                string messageName = context.GetMessage().GetType().Name;
                IMirandaMessageContext ctx = context.GetMessageContext() as IMirandaMessageContext;

                await Trace(context, token, messageName, messageId, ctx);
            }
        }

        private async Task Trace(IPipeContext context, CancellationToken token, string messageName, string messageId, IMirandaMessageContext ctx)
        {
            using (IScope scope = BuildScope(messageName, ctx.EnvoyCorrelationContext.OtSpanContext))
            {
                MirandaLogger.JaegerTracingPlugin.LogMessageContext(ctx.EnvoyCorrelationContext.OtSpanContext);
                ISpan span = scope.Span;
                span.Log($"Started processing: {messageName} [id: {messageId}]");
                MirandaLogger.JaegerTracingPlugin.LogPrecessingMessage(messageName, messageId);
                try
                {
                    _contextManager.CreateAndSet();
                    await Next.InvokeAsync(context, token);
                }
                catch (Exception ex)
                {
                    span.SetTag(Tags.Error, true);
                    span.Log(ex.Message);
                    MirandaLogger.JaegerTracingPlugin.LogExceptionWhileCreatingScope(ex.Message);
                }

                span.Log($"Finished processing: {messageName} [id: {messageId}]");
                MirandaLogger.JaegerTracingPlugin.LogFinishedProcessingMessage(messageName, messageId);
            }
        }

        private IScope BuildScope(string messageName, string serializedSpanContext)
        {
            ISpanBuilder spanBuilder = _tracer
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
    }
}
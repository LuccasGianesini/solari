using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Tag;

namespace Solari.Deimos.Jaeger
{
    public class DeimosJaegerTracer : IDeimosJaegerTracer
    {
        private readonly ITracer _tracer;
        private readonly ILogger<DeimosJaegerTracer> _logger;

        public DeimosJaegerTracer(ITracer tracer, ILogger<DeimosJaegerTracer> logger)
        {
            _tracer = tracer;
            _logger = logger;
        }

        
        public DeimosJaegerDescriptor StartSpan(string operationName, bool finishOnDispose = true)
        {
            _logger.LogDebug($"Creating child span for operation: {operationName}");
            return DeimosJaegerDescriptor.Create(_tracer.ScopeManager.Active, _tracer
                                                                               .BuildSpan(operationName)
                                                                               .AsChildOf(_tracer.ActiveSpan)
                                                                               .Start(), finishOnDispose);
        }


        public DeimosJaegerDescriptor StartTransaction(string operationName, bool finishOnDispose = true)
        {
            _logger.LogDebug("Opening scope for operation " + operationName);
            IScope scope = _tracer.BuildSpan(operationName).StartActive(finishOnDispose);
            _logger.LogDebug($"Opened scope with TraceId: {scope.Span.Context.TraceId}");
            return DeimosJaegerDescriptor.Create(scope);
        }

        public DeimosJaegerDescriptor StartTransaction(string operationName, HttpContext httpContext, bool finishOnDispose = true)
        {
            ISpanBuilder spanBuilder;

            try
            {
                if (!httpContext.Request.Headers.TryExtractContext(_tracer, out ISpanContext context))
                {
                    _logger.LogDebug($"Couldn't extract span headers. Operation: {operationName}. TraceId: {context.TraceId}");
                }

                spanBuilder = _tracer.BuildSpan(operationName);

                if (context != null)
                    spanBuilder.AsChildOf(context);
                else
                    return StartTransaction(operationName);
            }
            catch (Exception e)
            {
                _logger.LogError("An exception was thrown while trying to open a span from context." + e.Message);
                spanBuilder = _tracer.BuildSpan(operationName).WithTag(Tags.Error, true).WithTag("Error", e.Message);
            }

            return DeimosJaegerDescriptor.Create(spanBuilder.WithTag(Tags.SpanKind, Tags.SpanKindServer).StartActive(true));
        }
    }
}
using System;
using System.Collections.Generic;
using App.Metrics;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Tag;
using Solari.Ceres.Abstractions;

namespace Solari.Themis
{
    public class Themis : IThemis
    {
        private readonly IMetrics _metrics;
        private readonly ITracer _tracer;
        private readonly ILogger _logger;


        public Themis(IMetrics metrics, ITracer tracer, ILogger logger)
        {
            _metrics = metrics;
            _tracer = tracer;
            _logger = logger;
        }

        public ISpan TraceOperation(string operationName)
        {
            IScope activeScope = _tracer.ScopeManager.Active;
            ISpanBuilder spanBuilder = _tracer.BuildSpan(operationName);
            return activeScope?.Span is null
                       ? spanBuilder.StartActive(true).Span
                       : spanBuilder
                         .AsChildOf(_tracer.ScopeManager.Active.Span)
                         .Start();
        }

        public void TraceException(Exception exception, string customMessage = null, LogLevel level = LogLevel.Error)
        {
            //TODO TEST IF STACK TRACE IS LOGGED.
            _logger.Log(level, exception, string.IsNullOrEmpty(customMessage) ? exception.Message : customMessage);
            _metrics.Measure.Counter.Increment(MetricsRegistry.ErrorMetrics.CatchExceptionTotal);
            BuildExceptionSpan(TraceOperation("Exception"), exception);
        }


        private static ISpan BuildExceptionSpan(ISpan span, Exception exception)
        {
            return span.SetTag(Tags.Error, true)
                       .SetTag("catch", true)
                       .Log(ExtractExceptionInfo(exception));
        }

        private static IDictionary<string, object> ExtractExceptionInfo(Exception ex)
        {
            return new Dictionary<string, object>
            {
                {"Message", ex.Message},
                {"StackTrace", ex.StackTrace}
            };
        }
    }
}
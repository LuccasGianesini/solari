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
        private readonly IThemis _themis;

        public Themis(IMetrics metrics, ITracer tracer, ILogger<ThemisNoMetrics> logger)
        {
            _metrics = metrics;
            _tracer = tracer;
            _themis = new ThemisNoMetrics(tracer, logger);
        }

        public ISpan TraceOperation(string operationName)
        {
            return _themis.TraceOperation(operationName);
        }

        public void TraceException(Exception exception, string customMessage = null, LogLevel level = LogLevel.Error)
        {
            _themis.TraceException(exception, customMessage, level);
            _metrics.Measure.Counter.Increment(MetricsRegistry.ErrorMetrics.CatchExceptionTotal);
        }
    }
}
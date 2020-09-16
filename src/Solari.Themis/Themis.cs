using System;
using System.Collections.Generic;
using App.Metrics;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Tag;
using Solari.Ceres.Abstractions;
using Solari.Deimos;

namespace Solari.Themis
{
    public class Themis<T> : IThemis<T>
    {
        public ITracer Tracer { get; }
        public ILogger<T> Logger { get; }

        public Themis(ITracer tracer, ILogger<T> logger)
        {
            Tracer = tracer;
            Logger = logger;
        }

        public ISpan TraceOperation(string operationName)
        {
            IScope activeScope = Tracer.ScopeManager.Active;
            ISpanBuilder spanBuilder = Tracer.BuildSpan(operationName);
            return activeScope?.Span is null
                       ? spanBuilder.StartActive(true).Span
                       : spanBuilder
                         .AsChildOf(Tracer.ScopeManager.Active.Span)
                         .Start();
        }

        public void TraceException(string operationName, Exception exception, string logMessage = null, LogLevel level = LogLevel.Error)
        {
            Logger.Log(level, exception, string.IsNullOrEmpty(logMessage) ? exception.Message : logMessage);
            BuildExceptionSpan(TraceOperation(operationName), exception);
        }

        private static ISpan BuildExceptionSpan(ISpan span, Exception exception)
        {
            return span.SetTag(Tags.Error, true)
                       .SetTag("catch", true)
                       .Log(exception.ExtractExceptionsDetails());
        }

    }
}

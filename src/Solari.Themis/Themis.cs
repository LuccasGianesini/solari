using System;
using System.Collections.Generic;
using App.Metrics;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Tag;
using OpenTracing.Util;
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

        public ISpan TraceException(Exception exception, string logMessage = null, LogLevel level = LogLevel.Critical, params object[] args)
        {
            string errorMessage = string.IsNullOrEmpty(logMessage) ? exception.Message : logMessage;

            ISpan activeSpan = Tracer.ActiveSpan ?? TraceOperation(errorMessage);
            Logger.Log(level, exception, errorMessage, args);
            return BuildExceptionSpan(activeSpan, exception);
        }

        public ISpan TraceError(string logMessage, LogLevel level = LogLevel.Error, params object[] args)
        {
            ISpan activeSpan = Tracer.ActiveSpan ?? TraceOperation(logMessage);
            Logger.Log(level, logMessage, args);
            return activeSpan;
        }

        private static ISpan BuildExceptionSpan(ISpan span, Exception exception)
        {
            return span.SetTag(Tags.Error, true)
                       .SetTag("catch", true)
                       .Log(DateTimeOffset.UtcNow, exception.ExtractExceptionsDetails());
        }
    }
}

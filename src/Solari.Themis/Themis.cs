﻿using System;
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
        // private readonly IMetrics _metrics;
        private readonly ITracer _tracer;
        private readonly ILogger<Themis> _logger;

        public Themis(ITracer tracer, ILogger<Themis> logger)
        {
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
            _logger.Log(level, exception, string.IsNullOrEmpty(customMessage) ? exception.Message : customMessage);
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
                {"Message", ex.Message}, {"StackTrace", ex.StackTrace}
            };
        }
    }
}

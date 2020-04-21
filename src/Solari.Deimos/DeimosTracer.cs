using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTracing;
using OpenTracing.Tag;
using Solari.Deimos.Abstractions;

namespace Solari.Deimos
{
    public class DeimosTracer : IDeimosTracer
    {
        private readonly ITracer _tracer;
        private ConcurrentDictionary<string, ISpan> _spanCache;

        public DeimosTracer(ITracer tracer)
        {
            _tracer = tracer;
            _spanCache = new ConcurrentDictionary<string, ISpan>();
        }

        public ISpan TraceOperation(string operationName, Action<ISpanEnricher> enrich = null)
        {
            IScope activeScope = _tracer.ScopeManager.Active;
            ISpanBuilder spanBuilder = _tracer.BuildSpan(operationName);
            ISpan span = activeScope?.Span == null ? spanBuilder.Start() : spanBuilder.AsChildOf(_tracer.ScopeManager.Active.Span).Start();
            enrich?.Invoke(new SpanEnricher(span));
            _spanCache.TryAdd(Activity.Current.Id, span);
            return span;
        }

        public void FinalizeTrace(IDictionary<string, object> log = null)
        {
            if (!_spanCache.TryRemove(Activity.Current.Id, out ISpan span)) return;
            if (log != null)
            {
                span.Log(log);
            }

            span.Finish();
        }

        public void TraceException(Exception exception)
        {
            if (!_spanCache.TryGetValue(Activity.Current.Id, out ISpan span)) return;
            span.Log(ExtractExceptionInfo(exception));
            span.SetTag("catch", true)
                .SetTag(Tags.Error, true);
            span.Finish();
        }

        private IDictionary<string, object> ExtractExceptionInfo(Exception exception)
        {
            return new Dictionary<string, object>
            {
                {"Message", exception.Message},
                {"Source", exception.Source},
                {"Data", exception.Data},
                {"StackTrace", exception.StackTrace}
            };
        }
    }
}
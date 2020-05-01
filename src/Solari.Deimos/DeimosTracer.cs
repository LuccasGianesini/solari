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
        private ConcurrentDictionary<string, ISpan> _spanCache;

        public DeimosTracer(ITracer tracer)
        {
            OpenTracer = tracer;
            _spanCache = new ConcurrentDictionary<string, ISpan>();
        }

        public ITracer OpenTracer { get; }

        public ISpan TraceOperation(string operationName, Action<ISpanEnricher> enrich = null, bool createNewActivity = false)
        {
            var activeScope = OpenTracer.ScopeManager.Active;
            ISpanBuilder spanBuilder = OpenTracer.BuildSpan(operationName);
            ISpan span = activeScope?.Span == null ? spanBuilder.Start() : spanBuilder.AsChildOf(OpenTracer.ScopeManager.Active.Span).Start();
            enrich?.Invoke(new SpanEnricher(span));
            _spanCache.TryAdd(span.Context.SpanId, span);
            return span;
        }
        
        public void FinalizeTrace(ISpan spanToFinish , IDictionary<string, object> log = null)
        {
            if (!_spanCache.TryRemove(spanToFinish.Context.SpanId, out ISpan span)) return;
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
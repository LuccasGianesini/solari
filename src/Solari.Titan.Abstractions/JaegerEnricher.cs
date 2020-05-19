using OpenTracing;
using OpenTracing.Util;
using Serilog.Core;
using Serilog.Events;

namespace Solari.Titan.Abstractions
{
    public class JaegerEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            ITracer tracer = GlobalTracer.Instance;
            if(tracer is null)
                return;
            logEvent.AddPropertyIfAbsent(new LogEventProperty("JaegerTraceId", new ScalarValue(tracer.ActiveSpan?.Context?.TraceId)));
            logEvent.AddPropertyIfAbsent(new LogEventProperty("JaegerSpanId", new ScalarValue(tracer.ActiveSpan?.Context?.SpanId)));
            logEvent.AddPropertyIfAbsent(new LogEventProperty("JaegerSpanContext", new ScalarValue(tracer.ActiveSpan?.Context?.ToString())));

        }
    }
}
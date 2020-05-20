using Solari.Deimos.Abstractions;

namespace Solari.Deimos.CorrelationId
{
    public class DefaultEnvoyCorrelationContextContext : IEnvoyCorrelationContext
    {
        public bool IsValidEnvoyContext()
        {
            if (string.IsNullOrEmpty(TraceId))
                return false;
            if (string.IsNullOrEmpty(SpanId))
                return false;
            if (TraceId.Length != 32 || TraceId.Length != 16)
                return false;
            if (SpanId.Length != 16)
                return false;
            return ParentSpanId.Length == 16;
        }

        public string RequestId { get; set; }
        public string RequestIdHeader { get; } = DeimosConstants.RequestIdHeader;
        public string SpanId { get; set; }
        public string SpanIdHeader { get; } = DeimosConstants.EnvoySpanIdHeader;
        public string TraceId { get; set; }
        public string TraceIdHeader { get; } = DeimosConstants.EnvoyTraceIdHeader;
        public string ParentSpanId { get; set; }
        public string ParentSpanIdHeader { get; } = DeimosConstants.EnvoyParentSpanIdHeader;
        public string Sampled { get; set; }
        public string SampledHeader { get; } = DeimosConstants.EnvoySampledHeader;
        public string Flags { get; set; }
        public string FlagsHeader { get; } = DeimosConstants.EnvoyFlagsHeader;
        public string OtSpanContext { get; set; }
        public string OtSpanContextHeader { get; } = DeimosConstants.EnvoyOutgoingSpanContext;
    }
}
namespace Solari.Deimos.CorrelationId
{
    public interface IEnvoyCorrelationContext
    {
        bool IsValidEnvoyContext();
        string RequestId { get; set; }
        string RequestIdHeader { get; }
        string SpanId { get; set; }
        string SpanIdHeader { get; }
        string TraceId { get; set; }
        string TraceIdHeader { get; }
        string ParentSpanId { get; set; }
        string ParentSpanIdHeader { get; }
        string Sampled { get; set; }
        string SampledHeader { get; }
        string Flags { get; set; }
        string FlagsHeader { get; }
        string OtSpanContext { get; set; }
        string OtSpanContextHeader { get; }
    }
}
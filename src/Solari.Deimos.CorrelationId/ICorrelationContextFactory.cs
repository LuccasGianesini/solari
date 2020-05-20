namespace Solari.Deimos.CorrelationId
{
    public interface ICorrelationContextFactory
    {
        ICorrelationContext Create(IEnvoyCorrelationContext envoyCorrelationContext);
        ICorrelationContext Create(string messageId, IEnvoyCorrelationContext envoyCorrelationContext);
        ICorrelationContext CreateFromJaegerTracer(string messageId = "");

        IEnvoyCorrelationContext CreateEnvoy(string traceId, string spanId,
                                             string requestId = null, string parentSpanId = null,
                                             string sampled = "1", string flags = null,
                                             string outgoingSpanContext = null);
    }
}
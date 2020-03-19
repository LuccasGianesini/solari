namespace Solari.Deimos.CorrelationId
{
    public class HttpCorrelationContext : ICorrelationContext
    {
        public HttpCorrelationContext(string correlationId, string headerKey, string spanContext = null)
        {
            CorrelationId = correlationId;
            Header = headerKey;
            SpanContext = spanContext;
        }

        public string CorrelationId { get; set;}
        public string Header { get; set;}
        public string SpanContext { get; set;}
    }
}
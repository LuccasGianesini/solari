namespace Solari.Deimos.CorrelationId
{
    public class HttpCorrelationContext : ICorrelationContext
    {
        public HttpCorrelationContext(string correlationId, string headerKey)
        {
            CorrelationId = correlationId;
            Header = headerKey;
            Http = true;
            MessageBrokerName = string.Empty;
        }

        public string CorrelationId { get; }
        public string Header { get; }
        public bool Http { get; }
        public string MessageBrokerName { get; }
        
    }
}
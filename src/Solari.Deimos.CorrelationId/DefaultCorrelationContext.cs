namespace Solari.Deimos.CorrelationId
{
    public class DefaultCorrelationContext : ICorrelationContext
    {
        public DefaultCorrelationContext(string correlationId, string header, string broker)
        {
            CorrelationId = correlationId;
            Header = header;
            MessageBrokerName = broker;
            Http = false;
        }

        public string CorrelationId { get; }
        public string Header { get; }
        public bool Http { get; }
        public string MessageBrokerName { get; }

        
    }
}
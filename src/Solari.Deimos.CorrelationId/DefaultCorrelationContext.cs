using Solari.Deimos.Abstractions;

namespace Solari.Deimos.CorrelationId
{
    public class DefaultCorrelationContext : ICorrelationContext
    {
        public IEnvoyCorrelationContext EnvoyCorrelationContext { get; set; }
        public string MessageId { get; set; }
        public string MessageIdHeader { get; } = DeimosConstants.MessageIdHeader;
    }
}
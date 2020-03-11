using Solari.Deimos.Abstractions;

namespace Solari.Deimos.CorrelationId
{
    public class CorrelationContextFactory : ICorrelationContextFactory
    {
        public ICorrelationContext CreateHttpContext(string headerKey, string correlationId)
        {
            DeimosLogger.CorrelationIdLogger.CreatedHttpCorrelationContext(correlationId);
            return new HttpCorrelationContext(correlationId, headerKey);
        }

        public ICorrelationContext CreateBrokerContext(string headerKey, string correlationId, string messageBroker)
        {
            DeimosLogger.CorrelationIdLogger.CreatedBrokerCorrelationContext(correlationId);
            return new DefaultCorrelationContext(correlationId, headerKey, messageBroker);
        }
    }
}
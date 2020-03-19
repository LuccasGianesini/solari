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
        
    }
}
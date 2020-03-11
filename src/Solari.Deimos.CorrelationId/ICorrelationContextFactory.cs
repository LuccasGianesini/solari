namespace Solari.Deimos.CorrelationId
{
    public interface ICorrelationContextFactory
    {
        /// <summary>
        /// Create an <see cref="HttpCorrelationContext"/>.
        /// </summary>
        /// <param name="headerKey">The header key</param>
        /// <param name="correlationId">The header value</param>
        /// <returns></returns>
        ICorrelationContext CreateHttpContext(string headerKey, string correlationId);
        /// <summary>
        /// Create an <see cref="DefaultCorrelationContext"/>.
        /// </summary>
        /// <param name="headerKey">The header key</param>
        /// <param name="correlationId">The header value</param>
        /// <param name="messageBroker">The message broker</param>
        /// <returns></returns>
        ICorrelationContext CreateBrokerContext(string headerKey, string correlationId, string messageBroker);
    }
}
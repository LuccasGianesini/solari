namespace Solari.Deimos.CorrelationId
{
    public interface ICorrelationContext
    {
        /// <summary>
        /// The correlation id of the request.
        /// </summary>
        string CorrelationId { get; }
        /// <summary>
        /// The Header key. E.g.: X-Correlation-ID.
        /// </summary>
        string Header { get; }
        /// <summary>
        /// Indicate if it is an http request.
        /// </summary>
        bool Http { get; }
        /// <summary>
        /// The name of the message broker or name of the queue or topic or exchange.
        /// </summary>
        string MessageBrokerName { get; }
    }
}
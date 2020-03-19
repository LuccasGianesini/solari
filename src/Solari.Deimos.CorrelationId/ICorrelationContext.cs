namespace Solari.Deimos.CorrelationId
{
    public interface ICorrelationContext
    {
        /// <summary>
        /// The correlation id of the request.
        /// </summary>
        string CorrelationId { get; set; }
        /// <summary>
        /// The Header key. E.g.: X-Correlation-ID.
        /// </summary>
        string Header { get; set;}
        string SpanContext { get; set;}
    }
}
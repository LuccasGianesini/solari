namespace Solari.Deimos.CorrelationId
{
    public interface ICorrelationContextHandler
    {
        /// <summary>
        /// Current correlation context
        /// </summary>
        ICorrelationContext Current { get; set; }
    }
}
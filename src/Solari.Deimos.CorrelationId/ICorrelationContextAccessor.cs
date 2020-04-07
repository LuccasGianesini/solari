namespace Solari.Deimos.CorrelationId
{
    public interface ICorrelationContextAccessor
    {
        /// <summary>
        ///     Current correlation context
        /// </summary>
        ICorrelationContext Current { get; set; }
    }
}
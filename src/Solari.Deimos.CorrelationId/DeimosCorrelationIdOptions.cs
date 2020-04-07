namespace Solari.Deimos.CorrelationId
{
    public class DeimosCorrelationIdOptions
    {
        /// <summary>
        ///     <para>
        ///         Controls whether a GUID will be used in cases where no correlation ID is retrieved from the request header.
        ///         When false the TraceIdentifier for the current request will be used.
        ///     </para>
        ///     <para> Default: false.</para>
        /// </summary>
        public bool UseGuidForCorrelationId { get; set; } = false;
    }
}
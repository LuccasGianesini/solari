namespace Solari.Deimos.CorrelationId
{
    public interface ICorrelationContext
    {
        IEnvoyCorrelationContext EnvoyCorrelationContext { get; set; }

        string MessageId { get; set; }

        string MessageIdHeader { get; }
    }
}
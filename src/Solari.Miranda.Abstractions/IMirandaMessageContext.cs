using RawRabbit.Enrichers.MessageContext.Context;
using Solari.Deimos.CorrelationId;

namespace Solari.Miranda.Abstractions
{
    public interface IMirandaMessageContext : ICorrelationContext, IMessageContext
    {
        int Retries { get; set; }
        double Interval { get; set; }
    }
}
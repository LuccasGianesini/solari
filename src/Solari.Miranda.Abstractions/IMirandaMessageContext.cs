using RawRabbit.Enrichers.MessageContext.Context;
using Solari.Deimos.CorrelationId;

namespace Solari.Miranda.Abstractions
{
    public interface IMirandaMessageContext : ICorrelationContext, IMessageContext
    {
        string MessageId { get; set; }
    }
}
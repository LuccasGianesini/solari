using System;
using RawRabbit.Enrichers.MessageContext.Context;
using Solari.Deimos.Abstractions;
using Solari.Deimos.CorrelationId;
using Solari.Miranda.Abstractions;

namespace Solari.Miranda
{
    public class BrokerCorrelationContext : IMirandaMessageContext
    {
        public int Retries { get; set; }
        public double Interval { get; set; }
        public Guid GlobalRequestId { get; set; }
        public IEnvoyCorrelationContext EnvoyCorrelationContext { get; set; }
        public string MessageId { get; set; }
        public string MessageIdHeader { get; }
    }
}
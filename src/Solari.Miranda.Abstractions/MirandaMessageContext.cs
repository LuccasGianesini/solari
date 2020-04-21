using System;
using Solari.Deimos.CorrelationId;

namespace Solari.Miranda.Abstractions
{
    public class MirandaMessageContext : IMirandaMessageContext
    {
        public bool Empty { get; set; }
        public int Retries { get; set; }
        public double Interval { get; set; }
        public Guid GlobalRequestId { get; set; }
        public IEnvoyCorrelationContext EnvoyCorrelationContext { get; set; }
        public string MessageId { get; set; }
        public string MessageIdHeader { get; }
    }
}
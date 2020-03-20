using System;
using RawRabbit.Enrichers.MessageContext.Context;
using Solari.Deimos.Abstractions;
using Solari.Deimos.CorrelationId;
using Solari.Miranda.Abstractions;

namespace Solari.Miranda
{
    public class BrokerCorrelationContext : IMirandaMessageContext
    {
        public string MessageId { get; set; }
        public int Retries { get; set; }
        public double Interval { get; set; }
        public string CorrelationId { get; set; }
        public string Header { get; set; }
        public string SpanContext { get; set; }
        public Guid GlobalRequestId { get; set; }
        
        public static BrokerCorrelationContext Create(string correlationId = null, string messageId = null, string spanContext = null,
                                                      string header = DeimosConstants.DefaultCorrelationIdHeader)
        {
            var id = Guid.NewGuid();
            return new BrokerCorrelationContext
            {
                CorrelationId = string.IsNullOrEmpty(correlationId) ? new TraceIdGenerator().TraceIdentifier : correlationId,
                Header = header,
                MessageId = string.IsNullOrEmpty(messageId) ? id.ToString() : messageId,
                SpanContext = spanContext,
                GlobalRequestId = id
            };
        }
    }
}
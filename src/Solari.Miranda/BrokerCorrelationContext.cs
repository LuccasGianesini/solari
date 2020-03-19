using System;
using Solari.Deimos.CorrelationId;

namespace Solari.Miranda
{
    public class BrokerCorrelationContext : ICorrelationContext
    {
        public string CorrelationId { get; set;}
        public string Header { get; set;}
        public string SpanContext { get; set;}
    }
}
using RawRabbit.Configuration.Exchange;
using RawRabbit.Enrichers.Attributes;

namespace Solari.Samples.Domain
{
    [Exchange(Name = "test_class", Type = ExchangeType.Topic)]
    [Queue(Name = "test_class_queue.s")]
    [Routing(RoutingKey = "test_class.queue.s")]
    public class TestMessage
    {
        public string Value { get; set; }
    }
}
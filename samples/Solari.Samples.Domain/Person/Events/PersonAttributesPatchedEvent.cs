using System.Diagnostics;
using RawRabbit.Enrichers.Attributes;
using RawRabbit.Configuration.Exchange;
using Solari.Eris;

namespace Solari.Samples.Domain.Person.Events
{
    [Exchange(Name = "person", Type = ExchangeType.Topic)]
    [Routing(RoutingKey = "person.created")]
    [Queue(Durable = true, Exclusive = false, Name = "person.created")]
    public class PersonAttributesPatchedEvent : IEvent
    {
        public string PersonId { get; set; }

        public PersonAttributesPatchedEvent(string personId) { PersonId = personId; }
    }
}
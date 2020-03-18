using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Solari.Samples.Domain.Person.Events
{
    [Message(queue:"person-attributes")]
    public class PersonAttributesPatchedEvent : IEvent
    {
        public string PersonId { get; set; }

        public PersonAttributesPatchedEvent(string personId) { PersonId = personId; }
    }
}
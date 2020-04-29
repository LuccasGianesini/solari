using Solari.Eris;

namespace Solari.Samples.Domain.Person.Events
{
    public class PersonAttributesPatchedEvent : IEvent
    {
        public string PersonId { get; set; }

        public PersonAttributesPatchedEvent(string personId) { PersonId = personId; }
    }
}
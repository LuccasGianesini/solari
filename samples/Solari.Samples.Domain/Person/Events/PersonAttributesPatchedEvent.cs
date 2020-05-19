using Solari.Eris;

namespace Solari.Samples.Domain.Person.Events
{
    public class PersonAttributesPatchedEvent : IEvent
    {
        public PersonAttributesPatchedEvent(string personId) { PersonId = personId; }
        public string PersonId { get; set; }
    }
}
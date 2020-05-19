using Solari.Eris;
using Solari.Samples.Domain.Person.Results;

namespace Solari.Samples.Domain.Person.Events
{
    public class PersonCreatedEvent : IEvent
    {
        public PersonCreatedEvent(CreatePersonResult result) { Result = result; }
        public CreatePersonResult Result { get; }
    }
}
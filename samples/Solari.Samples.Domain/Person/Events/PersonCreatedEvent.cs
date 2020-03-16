using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using MongoDB.Bson;
using Solari.Samples.Domain.Person.Results;

namespace Solari.Samples.Domain.Person.Events
{
    public class PersonCreatedEvent: IEvent

    {
        public CreatePersonResult Result { get; }


        public PersonCreatedEvent(CreatePersonResult result) { Result = result; }
    }
}
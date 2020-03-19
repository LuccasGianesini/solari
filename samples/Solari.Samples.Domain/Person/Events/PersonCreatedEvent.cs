using System;
using EasyNetQ;
using Solari.Eris;
using Solari.Miranda;
using Solari.Samples.Domain.Person.Results;

namespace Solari.Samples.Domain.Person.Events
{
    public class PersonCreatedEventMessage : IMessage
    {
        public object GetBody() { throw new NotImplementedException(); }

        public MessageProperties Properties { get; }
        public Type MessageType { get; }
    }
    public class PersonCreatedEvent : IEvent
    {
        public CreatePersonResult Result { get; }

        public PersonCreatedEvent(CreatePersonResult result) { Result = result; }
    }
}
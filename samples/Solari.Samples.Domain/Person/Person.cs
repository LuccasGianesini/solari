using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person.Commands;

namespace Solari.Samples.Domain.Person
{
    public class Person : IDocumentRoot, IInsertableDocument, IUpdatableDocument<Person>
    {
        public Person(string name)
        {
            Name = name;
            Attributes = new List<PersonAttribute>(2);
            CreatedAt = DateTimeOffset.Now;
            PendingUpdates = new Queue<UpdateOneModel<Person>>();
        }

        public Guid Id { get; set; }
        public string Address { get; protected set; }
        public string Name { get; protected set; }
        public DateTimeOffset CreatedAt { get; protected set; }
        public List<PersonAttribute> Attributes { get; protected set; }

        public Person AddAttributes(IEnumerable<PersonAttribute> attributes)
        {
            Attributes.AddRange(attributes);
            PendingUpdates.Enqueue(new UpdateOneModel<Person>(null, Builders<Person>.Update.PushEach(a => a.Attributes, attributes)));
            return this;
        }

        public Person AddAttribute(PersonAttribute attribute)
        {
            Attributes.Add(attribute);
            return this;
        }

        public static explicit operator Person(CreatePersonCommand command)
        {
            var person = new Person(command.Name);
            if (command.Attributes != null)
            {
                person.AddAttributes(command.Attributes?.Select(a => (PersonAttribute) a));
            }

            return person;
        }

        public Queue<UpdateOneModel<Person>> PendingUpdates { get; }
    }
}

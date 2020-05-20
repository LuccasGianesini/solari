using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Solari.Callisto.Abstractions;
using Solari.Samples.Domain.Person.Commands;

namespace Solari.Samples.Domain.Person
{
    public class Person : IDocumentRoot
    {
        public Person(string name)
        {
            Name = name;
            Attributes = new List<PersonAttribute>(2);
            CreatedAt = DateTimeOffset.Now;
        }

        public string Address { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public List<PersonAttribute> Attributes { get; set; }
        public ObjectId Id { get; set; }

        public Person AddAttributes(IEnumerable<PersonAttribute> attributes)
        {
            Attributes.AddRange(attributes);
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
    }
}
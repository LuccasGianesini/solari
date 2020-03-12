using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Solari.Callisto.Abstractions;
using Solari.Samples.Domain.Person.Dtos;

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

        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public ObjectId Id { get; set; }

        public List<PersonAttribute> Attributes { get; set; }

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
        public static explicit operator Person(InsertPersonDto dto)
        {
            var person = new Person(dto.Name);
            if (dto.Attributes != null)
            {
                person.AddAttributes(dto.Attributes?.Select(a => (PersonAttribute) a));
            }

            return person;
        }
    }
}
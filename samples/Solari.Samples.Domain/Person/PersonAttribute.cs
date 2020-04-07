using System;
using Solari.Callisto.Abstractions;
using Solari.Samples.Domain.Person.Commands;
using Solari.Samples.Domain.Person.Dtos;

namespace Solari.Samples.Domain.Person
{
    public class PersonAttribute : IDocumentNode
    {
        public PersonAttribute(string attributeName, string attributeValue)
        {
            AttributeName = attributeName;
            AttributeValue = attributeValue;
        }

        public string AttributeName { get; set; }

        public string AttributeValue { get; set; }

        public static explicit operator PersonAttribute(PersonAttributeDto command)
        {
            return new PersonAttribute(command.AttributeName, command.AttributeValue);
        }
        }
}
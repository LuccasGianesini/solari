using System;
using Solari.Callisto.Abstractions;
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

        public static explicit operator PersonAttribute(PersonAddAttributeDto dto)
        {
            if (string.IsNullOrEmpty(dto.AttributeName))
                throw new ArgumentNullException(nameof(dto.AttributeName), "Attribute name cannot be null or empty");
            if (string.IsNullOrEmpty(dto.AttributeValue))
                throw new ArgumentNullException(nameof(dto.AttributeValue), "Attribute value cannot be null or empty");
            return new PersonAttribute(dto.AttributeName, dto.AttributeValue);
        }
    }
}
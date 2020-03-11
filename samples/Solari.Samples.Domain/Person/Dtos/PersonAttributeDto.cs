using MongoDB.Bson;

namespace Solari.Samples.Domain.Person.Dtos
{
    public class PersonAddAttributeDto
    {
        public ObjectId PersonId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }
}
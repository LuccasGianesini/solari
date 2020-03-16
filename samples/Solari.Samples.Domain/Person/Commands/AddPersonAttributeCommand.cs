using MongoDB.Bson;

namespace Solari.Samples.Domain.Person.Commands
{
    public class AddPersonAttributeCommand
    {
        public ObjectId PersonId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }
}
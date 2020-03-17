using Convey.CQRS.Commands;
using MongoDB.Bson;
using Solari.Samples.Domain.Person.Results;

namespace Solari.Samples.Domain.Person.Commands
{
    public class PersonAttributeCommand : ICommand
    {
        public string PersonId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        
        public bool UpdateIfPresent { get; set; }
        public AddPersonAttributeResult Result { get; set; }
        
        public ObjectId ObjectId => MongoDB.Bson.ObjectId.Parse(PersonId);
        
    }
}
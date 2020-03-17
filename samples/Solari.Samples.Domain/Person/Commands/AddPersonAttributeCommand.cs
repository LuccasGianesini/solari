using Convey.CQRS.Commands;
using MongoDB.Bson;
using Solari.Samples.Domain.Person.Results;

namespace Solari.Samples.Domain.Person.Commands
{
    public class AddPersonAttributeCommand : ICommand
    {
        public ObjectId PersonId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        
        public AddPersonAttributeResult Result { get; set; }
        
    }
}
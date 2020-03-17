using Convey.CQRS.Events;
using MongoDB.Bson;

namespace Solari.Samples.Domain.Person.Events
{
    public class AttributeAddedEvent : IEvent
    {
        public ObjectId PersonId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }

        public AttributeAddedEvent(ObjectId personId, string attributeName, string attributeValue)
        {
            PersonId = personId;
            AttributeName = attributeName;
            AttributeValue = attributeValue;
        }
    }
}
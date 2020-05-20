using MongoDB.Bson;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person.Commands;
using Solari.Samples.Domain.Person.Dtos;

namespace Solari.Samples.Domain.Person
{
    public interface IPersonOperations
    {
        ICallistoInsert<Person> CreateInsertOperation(CreatePersonCommand createPersonCommand);
        ICallistoUpdate<Person> CreateRemoveAttributeOperation(ObjectId id, PersonAttributeDto command);
        ICallistoUpdate<Person> CreateUpdateAttributeOperation(ObjectId id, PersonAttributeDto command);
        ICallistoUpdate<Person> CreateAddAttributeOperation(ObjectId id, PersonAttributeDto command);
        ICallistoUpdate<Person> CreateUpdatePersonOperation(string id, ICallistoUpdate update);
    }
}
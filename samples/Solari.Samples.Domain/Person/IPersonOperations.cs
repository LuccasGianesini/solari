using System;
using MongoDB.Bson;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person.Commands;
using Solari.Samples.Domain.Person.Dtos;

namespace Solari.Samples.Domain.Person
{
    public interface IPersonOperations
    {
        ICallistoInsertOne<Person> CreateInsertOperation(CreatePersonCommand createPersonCommand);
        ICallistoUpdate<Person> CreateRemoveAttributeOperation(Guid id, PersonAttributeDto command);
        ICallistoUpdate<Person> CreateUpdateAttributeOperation(Guid id, PersonAttributeDto command);
        ICallistoUpdate<Person> CreateAddAttributeOperation(Guid id, PersonAttributeDto command);
    }
}
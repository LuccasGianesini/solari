using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Dtos;

namespace Solari.Samples.Infrastructure
{
    public class PersonOperations : IPersonOperations
    {
        private readonly ICallistoOperationFactory _operationFactory;

        public PersonOperations(ICallistoOperationFactory operationFactory) { _operationFactory = operationFactory; }
        
        public ICallistoInsert<Person> CreateInsertOperation(InsertPersonDto insertPersonDto)
        {
            return _operationFactory.CreateInsert("insert-person", (Person) insertPersonDto);
        }

        public ICallistoUpdate<Person> CreateAddAttributeOperation(PersonAddAttributeDto dto)
        {
            UpdateDefinition<Person> update = Builders<Person>.Update.Push(a => a.Attributes, (PersonAttribute) dto);
            return _operationFactory.CreateUpdateById("add-person-attribute", dto.PersonId, update);
        }
        
    }
}
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Commands;
using Solari.Titan;

namespace Solari.Samples.Infrastructure
{
    public class PersonOperations : IPersonOperations
    {
        private readonly ICallistoOperationFactory _operationFactory;
        private readonly ITitanLogger<PersonOperations> _logger;

        public PersonOperations(ICallistoOperationFactory operationFactory, ITitanLogger<PersonOperations> logger)
        {
            _operationFactory = operationFactory;
            _logger = logger;
        }
        
        public ICallistoInsert<Person> CreateInsertOperation(CreatePersonCommand createPersonCommand)
        {
            _logger.Information($"Creating operation: {PersonConstants.CreateOperationName}");
            return _operationFactory.CreateInsert(PersonConstants.CreateOperationName, (Person) createPersonCommand);
        }

        public ICallistoUpdate<Person> CreateAddAttributeOperation(AddPersonAttributeCommand command)
        {
            UpdateDefinition<Person> update = Builders<Person>.Update.Push(a => a.Attributes, (PersonAttribute) command);
            return _operationFactory.CreateUpdateById("add-person-attribute", command.PersonId, update);
        }
        
    }
}
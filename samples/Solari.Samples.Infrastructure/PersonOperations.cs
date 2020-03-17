using System.Linq;
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
            _logger.Information("Creating callisto operation object");
            return _operationFactory.CreateInsert(PersonConstants.CreatePersonOperationName, (Person) createPersonCommand);
        }

        public ICallistoUpdate<Person> CreateAddAttributeOperation(PersonAttributeCommand command)
        {
            _logger.Information("Creating callisto operation object");
            UpdateDefinition<Person> update = Builders<Person>.Update.Push(a => a.Attributes, (PersonAttribute) command);
            return _operationFactory.CreateUpdateById(PersonConstants.AddAttributeToPersonOperationName, command.ObjectId, update);
        }

        public ICallistoUpdate<Person> CreateUpdateAttributeOperation(PersonAttributeCommand command)
        {
            _logger.Information("Creating callisto operation object");
            FilterDefinition<Person> filter = Builders<Person>.Filter.Where(a => a.Id == command.ObjectId 
                                                                              && a.Attributes.Any(at => at.AttributeName == command.AttributeName));
            UpdateDefinition<Person> update = Builders<Person>.Update.Set(x => x.Attributes[-1].AttributeValue, command.AttributeValue);
            return _operationFactory.CreateUpdate(PersonConstants.UpdatePersonAttributeOperationName, update, filter);
        }
        
    }
}
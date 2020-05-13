using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Commands;
using Solari.Samples.Domain.Person.Dtos;
using Solari.Titan;

namespace Solari.Samples.Infrastructure
{
    public class PersonOperations : IPersonOperations
    {
        private readonly ILogger<PersonOperations> _logger;
        private readonly ICallistoOperationFactory _operationFactory;

        public PersonOperations(ICallistoOperationFactory operationFactory, ILogger<PersonOperations> logger)
        {
            _operationFactory = operationFactory;
            _logger = logger;
        }

        public ICallistoUpdate<Person> CreateUpdatePersonOperation(string id, ICallistoUpdate update)
        {
            return null;
        }

        public ICallistoInsert<Person> CreateInsertOperation(CreatePersonCommand createPersonCommand)
        {
            _logger.LogInformation("Creating callisto operation object");
            return _operationFactory.CreateInsert((Person) createPersonCommand);
        }

        public ICallistoUpdate<Person> CreateAddAttributeOperation(ObjectId id, PersonAttributeDto command)
        {
            _logger.LogInformation("Creating callisto operation object");
            UpdateDefinition<Person> update = Builders<Person>.Update.Push(a => a.Attributes, (PersonAttribute) command);
            return _operationFactory.CreateUpdateById(id, update);
        }

        public ICallistoUpdate<Person> CreateUpdateAttributeOperation(ObjectId id, PersonAttributeDto command)
        {
            _logger.LogInformation("Creating callisto operation object");
            FilterDefinition<Person> filter = Builders<Person>.Filter.Where(a => a.Id == id && a.Attributes
                                                                                                .Any(at => at.AttributeName == command.AttributeName));
            UpdateDefinition<Person> update = Builders<Person>.Update.Set(x => x.Attributes[-1].AttributeValue, command.AttributeValue);
            return _operationFactory.CreateUpdate(update, filter);
        }

        public ICallistoUpdate<Person> CreateRemoveAttributeOperation(ObjectId id, PersonAttributeDto command)
        {
            _logger.LogInformation("Creating callisto operation object");
            FilterDefinition<PersonAttribute> pullFilter = Builders<PersonAttribute>.Filter.Eq(a => a.AttributeName, command.AttributeName);
            UpdateDefinition<Person> update = Builders<Person>.Update.PullFilter(a => a.Attributes, pullFilter);
            return _operationFactory.CreateUpdateById(id, update);
        }
    }
}
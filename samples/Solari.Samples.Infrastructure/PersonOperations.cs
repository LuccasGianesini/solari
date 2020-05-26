using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Framework;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Commands;
using Solari.Samples.Domain.Person.Dtos;

namespace Solari.Samples.Infrastructure
{
    public class PersonOperations : IPersonOperations
    {
        private readonly ICallistoInsertOperationFactory _insertOperationFactory;
        private readonly ICallistoUpdateOperationFactory _updateOperationFactory;
        private readonly ILogger<PersonOperations> _logger;

        public PersonOperations(ICallistoInsertOperationFactory insertOperationFactory,
                                ICallistoUpdateOperationFactory updateOperationFactory,
                                ILogger<PersonOperations> logger)
        {
            _insertOperationFactory = insertOperationFactory;
            _updateOperationFactory = updateOperationFactory;
            _logger = logger;
        }

        public ICallistoInsertOne<Person> CreateInsertOperation(CreatePersonCommand createPersonCommand)
        {
            _logger.LogInformation("Creating callisto operation object");
            return _insertOperationFactory.CreateInsertOne((Person) createPersonCommand);
        }
        
        public ICallistoUpdate<Person> CreateAddAttributeOperation(Guid id, PersonAttributeDto command)
        {
            _logger.LogInformation("Creating callisto operation object");
            UpdateDefinition<Person> update = Builders<Person>.Update.Push(a => a.Attributes, (PersonAttribute) command);
            return _updateOperationFactory.CreateUpdateById(id, update);
        }
        
        public ICallistoUpdate<Person> CreateUpdateAttributeOperation(Guid id, PersonAttributeDto command)
        {
            _logger.LogInformation("Creating callisto operation object");
            FilterDefinition<Person> filter = Builders<Person>.Filter.Where(a => a.Id == id && a.Attributes
                                                                                                .Any(at => at.AttributeName == command.AttributeName));
            UpdateDefinition<Person> update = Builders<Person>.Update.Set(x => x.Attributes[-1].AttributeValue, command.AttributeValue);
            return _updateOperationFactory.CreateUpdate(update, filter);
        }
        
        public ICallistoUpdate<Person> CreateRemoveAttributeOperation(Guid id, PersonAttributeDto command)
        {
            _logger.LogInformation("Creating callisto operation object");
            FilterDefinition<PersonAttribute> pullFilter = Builders<PersonAttribute>.Filter.Eq(a => a.AttributeName, command.AttributeName);
            UpdateDefinition<Person> update = Builders<Person>.Update.PullFilter(a => a.Attributes, pullFilter);
            return _updateOperationFactory.CreateUpdateById(id, update);
        }
    }
}
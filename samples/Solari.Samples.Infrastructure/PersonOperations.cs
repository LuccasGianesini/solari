using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jaeger.Thrift.Agent;
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
        private readonly ICallistoOperationFactory _operationFactory;
        private readonly ITitanLogger<PersonOperations> _logger;

        public PersonOperations(ICallistoOperationFactory operationFactory, ITitanLogger<PersonOperations> logger)
        {
            _operationFactory = operationFactory;
            _logger = logger;
        }

        public ICallistoUpdate<Person> CreateUpdatePersonOperation(string id, ICallistoUpdate update)
        {
            var props = update.GetType()
                              .GetProperties()
                              .Where(a => update.Fields
                                                .Select(b => b.ToLowerInvariant())
                                                .Contains(a.Name.ToLowerInvariant()))
                              .ToList();

            foreach (PropertyInfo propertyInfo in props)
            {
                var name = propertyInfo.Name;
                var val = propertyInfo.GetValue(update);
            }

            return null;
        }

        public ICallistoInsert<Person> CreateInsertOperation(CreatePersonCommand createPersonCommand)
        {
            _logger.Information("Creating callisto operation object");
            return _operationFactory.CreateInsert(PersonConstants.CreatePersonOperationName, (Person) createPersonCommand);
        }

        public ICallistoUpdate<Person> CreateAddAttributeOperation(ObjectId id, PersonAttributeDto command)
        {
            _logger.Information("Creating callisto operation object");
            UpdateDefinition<Person> update = Builders<Person>.Update.Push(a => a.Attributes, (PersonAttribute) command);
            return _operationFactory.CreateUpdateById(PersonConstants.AddAttributeToPersonOperationName, id, update);
        }

        public ICallistoUpdate<Person> CreateUpdateAttributeOperation(ObjectId id, PersonAttributeDto command)
        {
            _logger.Information("Creating callisto operation object");
            FilterDefinition<Person> filter = Builders<Person>.Filter.Where(a => a.Id == id && a.Attributes
                                                                                                .Any(at => at.AttributeName == command.AttributeName));
            UpdateDefinition<Person> update = Builders<Person>.Update.Set(x => x.Attributes[-1].AttributeValue, command.AttributeValue);
            return _operationFactory.CreateUpdate(PersonConstants.UpdatePersonAttributeOperationName, update, filter);
        }

        public ICallistoUpdate<Person> CreateRemoveAttributeOperation(ObjectId id, PersonAttributeDto command)
        {
            _logger.Information("Creating callisto operation object");
            FilterDefinition<PersonAttribute> pullFilter = Builders<PersonAttribute>.Filter.Eq(a => a.AttributeName, command.AttributeName);
            UpdateDefinition<Person> update = Builders<Person>.Update.PullFilter(a => a.Attributes, pullFilter);
            return _operationFactory.CreateUpdateById(PersonConstants.RemovePersonAttributeOperationName, id, update);
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person.Events;
using Solari.Samples.Domain.Person.Results;
using Solari.Titan;

namespace Solari.Samples.Domain.Person.Commands.Handlers
{
    public class AddPersonAttributeCommandHandler : ICommandHandler<PersonAttributeCommand>
    {
        private readonly ITitanLogger<AddPersonAttributeCommandHandler> _logger;
        private readonly IEventDispatcher _dispatcher;
        private readonly IPersonRepository _repository;
        private readonly IPersonOperations _operations;

        public AddPersonAttributeCommandHandler(ITitanLogger<AddPersonAttributeCommandHandler> logger, IEventDispatcher dispatcher,
                                                IPersonRepository repository, IPersonOperations operations)
        {
            _logger = logger;
            _dispatcher = dispatcher;
            _repository = repository;
            _operations = operations;
        }

        public async Task HandleAsync(PersonAttributeCommand command)
        {
            _logger.Information($"Received '{PersonConstants.AddAttributeToPersonOperationName}' command with person id: {command.PersonId}");
            Person personFromDb = await _repository.Get(command.ObjectId);
            if (personFromDb == null)
            {
                throw new InvalidOperationException("The provided person id did not match any record in the database");
            }

            if (personFromDb.Attributes.Any(a => a.AttributeName == command.AttributeName))
            {
                throw new InvalidOperationException("Person already contains the provided attribute. To update use the PATCH endpoint");
            }

            ICallistoUpdate<Person> operation = _operations.CreateAddAttributeOperation(command);


            AddPersonAttributeResult repositoryResult = await _repository.AddAttribute(operation);
            if (repositoryResult.Success)
            {
                _logger.Information($"Successfully added attribute {command.AttributeName} to person {command.PersonId}");
                await _dispatcher.PublishAsync(new AttributeAddedEvent(command.ObjectId, command.AttributeName, command.AttributeValue));
            }
            else
            {
                _logger.Error($"Error while adding attribute {command.AttributeName} to person {command.PersonId}");
            }


            command.Result = repositoryResult;
        }
    }
}
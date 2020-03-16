using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Elastic.Apm;
using Elastic.Apm.Api;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person.Commands;
using Solari.Samples.Domain.Person.Events;
using Solari.Samples.Domain.Person.Results;
using Solari.Titan;

namespace Solari.Samples.Domain.Person
{
    public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand>
    {
        private readonly ITitanLogger<CreatePersonCommandHandler> _logger;
        private readonly IEventDispatcher _dispatcher;
        private readonly IPersonRepository _repository;
        private readonly IPersonOperations _operations;

        public CreatePersonCommandHandler(ITitanLogger<CreatePersonCommandHandler> logger, IEventDispatcher dispatcher,
                                          IPersonRepository repository, IPersonOperations operations)
        {
            _logger = logger;
            _dispatcher = dispatcher;
            _repository = repository;
            _operations = operations;
        }

        public async Task HandleAsync(CreatePersonCommand command)
        {
            _logger.Information("Creating a new person....");
            ICallistoInsert<Person> operation = _operations.CreateInsertOperation(command);
            _logger.Information($"Successfully created {PersonConstants.CreateOperationName} operation.");
            CreatePersonResult dataBaseResult = await _repository.InsertPerson(operation);
            if (dataBaseResult.Success)
            {
                _logger.Information($"A new database registry was created. id: '{dataBaseResult.Id}'");
            }
            else
            {
                _logger.Error($"Error while trying to save person into the database");
            }
            await _dispatcher.PublishAsync(new PersonCreatedEvent(dataBaseResult));
            command.Result = dataBaseResult;

        }
    }
}
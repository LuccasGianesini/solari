using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person.Events;
using Solari.Samples.Domain.Person.Results;
using Solari.Titan;

namespace Solari.Samples.Domain.Person.Commands.Handlers
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
            _logger.Information($"Received {PersonConstants.CreatePersonOperationName} command");
            ICallistoInsert<Person> operation = _operations.CreateInsertOperation(command);
            CreatePersonResult repositoryResult = await _repository.InsertPerson(operation);
            if (repositoryResult.Success)
            {
                _logger.Information($"A new database registry was created. id: '{repositoryResult.Id}'");
                await _dispatcher.PublishAsync(new PersonCreatedEvent(repositoryResult));
            }
            else
            {
                _logger.Error($"Error while trying to save person into the database");
            }
            
            command.Result = repositoryResult;

        }
    }
}
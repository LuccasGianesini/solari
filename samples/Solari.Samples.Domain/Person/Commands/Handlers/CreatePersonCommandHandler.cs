using System.Threading.Tasks;
using Solari.Callisto.Abstractions.CQR;
using Solari.Eris;
using Solari.Samples.Domain.Person.Events;
using Solari.Samples.Domain.Person.Results;
using Solari.Titan;

namespace Solari.Samples.Domain.Person.Commands.Handlers
{
    public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand>
    {
        private readonly IDispatcher _dispatcher;
        private readonly ITitanLogger<CreatePersonCommandHandler> _logger;
        private readonly IPersonOperations _operations;
        private readonly IPersonRepository _repository;

        public CreatePersonCommandHandler(ITitanLogger<CreatePersonCommandHandler> logger, IDispatcher dispatcher,
                                          IPersonRepository repository, IPersonOperations operations)
        {
            _logger = logger;
            _dispatcher = dispatcher;
            _repository = repository;
            _operations = operations;
        }

        public async Task HandleCommandAsync(CreatePersonCommand command)
        {
            Helper.DefaultCommandLogMessage(_logger, PersonConstants.CreatePersonOperationName);

            ICallistoInsert<Person> operation = _operations.CreateInsertOperation(command);
            CreatePersonResult repositoryResult = await _repository.InsertPerson(operation);
            if (repositoryResult.Success)
            {
                _logger.Information($"A new database registry was created. id: '{repositoryResult.Id}'");
                await _dispatcher.DispatchEvent(new PersonCreatedEvent(repositoryResult));
            }
            else
            {
                _logger.Error("Error while trying to save person into the database");
            }

            command.Result = repositoryResult;
        }
    }
}
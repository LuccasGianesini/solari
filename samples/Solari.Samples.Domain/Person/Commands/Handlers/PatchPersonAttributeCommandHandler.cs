using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.CQR;
using Solari.Eris;
using Solari.Samples.Domain.Person.Dtos;
using Solari.Titan;
using Solari.Vanth;
using Solari.Vanth.Builders;

namespace Solari.Samples.Domain.Person.Commands.Handlers
{
    public class AddPersonAttributeCommandHandler : ICommandHandler<PersonAttributeCommand>
    {
        private readonly IDispatcher _dispatcher;
        private readonly ITitanLogger<AddPersonAttributeCommandHandler> _logger;
        private readonly IPersonOperations _operations;
        private readonly IPersonRepository _repository;

        public AddPersonAttributeCommandHandler(ITitanLogger<AddPersonAttributeCommandHandler> logger, IDispatcher dispatcher,
                                                IPersonRepository repository, IPersonOperations operations)
        {
            _logger = logger;
            _dispatcher = dispatcher;
            _repository = repository;
            _operations = operations;
        }

        public async Task HandleCommandAsync(PersonAttributeCommand command)
        {
            Helper.DefaultCommandLogMessage(_logger, PersonConstants.AddAttributeToPersonOperationName, $" with person id: {command.PersonId}");
            Person personFromDb = await _repository.Get(command.ObjectId);
            if (personFromDb == null) throw new InvalidOperationException("The provided person id did not match any record in the database");

            CommonResponse<object> response = await ExecutePatch(command);

            await FinalActions(command, response);

            command.Result = response;
        }

        private async Task FinalActions(PersonAttributeCommand command, CommonResponse<object> response)
        {
            // if (response.HasResult) await _dispatcher.PublishAsync(new PersonAttributesPatchedEvent(command.PersonId));

            if (response.HasErrors) _logger.Warning($"Error while patching person attributes. Person id: {command.PersonId}");
        }

        private async Task<CommonResponse<object>> ExecutePatch(PersonAttributeCommand command)
        {
            const string failed = "Filed to execute db write. Possible invalid command operation";
            long modified = 0;
            var response = new CommonResponseBuilder<object>();
            foreach (PersonAttributeDto personAttributeDto in command.Values)
            {
                UpdateResult repositoryResult = await _repository.PatchAttribute(CreateUpdateOperation(command.Operation,
                                                                                                       command.ObjectId,
                                                                                                       personAttributeDto));

                if (!repositoryResult.IsAcknowledged || repositoryResult.ModifiedCount != 1)
                    WhenNotModified(failed, personAttributeDto, response);
                else
                    modified += WhenModified(command, personAttributeDto);
            }

            if (modified != 0) response.WithResult(new {Modified = modified});

            return response.Build();
        }

        private long WhenModified(PersonAttributeCommand command, PersonAttributeDto personAttributeDto)
        {
            _logger.Information($"Successfully {command.Operation.ToString()} person {command.PersonId} attribute. " +
                                $"Attribute: {personAttributeDto.AttributeName}");
            return 1;
        }

        private void WhenNotModified(string failed, PersonAttributeDto personAttributeDto, ICommonResponseBuilder<object> response)
        {
            _logger.Warning(failed, enricher => enricher.WithProperty("Target", personAttributeDto.AttributeName));
            response.WithError(builder => builder.WithMessage(failed)
                                                 .WithTarget(personAttributeDto.AttributeName)
                                                 .WithErrorType("Database Write error")
                                                 .Build());
        }

        private ICallistoUpdate<Person> CreateUpdateOperation(PatchOperation operation, ObjectId id, PersonAttributeDto dto)
        {
            return operation switch
                   {
                       PatchOperation.Add    => _operations.CreateAddAttributeOperation(id, dto),
                       PatchOperation.Remove => _operations.CreateRemoveAttributeOperation(id, dto),
                       PatchOperation.Update => _operations.CreateUpdateAttributeOperation(id, dto),
                       _                     => throw new ArgumentOutOfRangeException(nameof(PatchOperation), "An invalid patch operation was provided")
                   };
        }
    }
}
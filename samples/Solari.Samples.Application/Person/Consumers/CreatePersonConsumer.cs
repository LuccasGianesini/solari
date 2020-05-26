using System.Threading.Tasks;
using MassTransit;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Commands;
using Solari.Samples.Domain.Person.Events;
using Solari.Samples.Domain.Person.Results;

namespace Solari.Samples.Application.Person.Consumers
{
    public class CreatePersonConsumer : IConsumer<CreatePersonCommand>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPersonOperations _operations;

        public CreatePersonConsumer(IPersonRepository personRepository, IPersonOperations operations)
        {
            _personRepository = personRepository;
            _operations = operations;
        }
        public async Task Consume(ConsumeContext<CreatePersonCommand> context)
        {
            ICallistoInsertOne<Domain.Person.Person> insertOperation = _operations.CreateInsertOperation(context.Message);
            CreatePersonResult dbResult = await _personRepository.InsertPerson(insertOperation);
            await context.RespondAsync<IPersonCreatedEvent>(new
            {
                Result = dbResult
            });


        }
    }
}
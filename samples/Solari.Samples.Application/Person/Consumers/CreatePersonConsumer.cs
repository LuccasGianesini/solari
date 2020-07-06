using System.Threading.Tasks;
using MassTransit;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Commands;
using Solari.Samples.Domain.Person.Events;
using Solari.Samples.Domain.Person.Results;

namespace Solari.Samples.Application.Person.Consumers
{
    public class CreatePersonConsumer : IConsumer<CreatePersonCommand>
    {
        private readonly IPersonCollection _personCollection;
        private readonly IPersonOperations _operations;

        public CreatePersonConsumer(IPersonCollection personCollection, IPersonOperations operations)
        {
            _personCollection = personCollection;
            _operations = operations;
        }

        public async Task Consume(ConsumeContext<CreatePersonCommand> context)
        {
            ICallistoInsertOne<Domain.Person.Person> insertOperation = _operations.CreateInsertOperation(context.Message);
            CreatePersonResult dbResult = await _personCollection.InsertPerson(insertOperation);
            if (dbResult.Success)
            {
                await context.Publish<IPersonCreatedEvent>(new
                {
                    Result = dbResult
                });
            }

            await context.RespondAsync<IPersonCreatedEvent>(new
            {
                Result = dbResult
            });
        }
    }
}
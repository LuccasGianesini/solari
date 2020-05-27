using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Solari.Callisto;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Results;

namespace Solari.Samples.Infrastructure
{
    public class PersonRepository : CallistoRepository<Person>, IPersonRepository
    {
        private readonly ILogger<PersonRepository> _logger;
        public PersonRepository(ICallistoContext<Person> context, ILogger<PersonRepository> logger) : base(context)
        { _logger = logger; }

        public async Task<CreatePersonResult> InsertPerson(ICallistoInsertOne<Person> insertOne)
        {
            await Insert.One(insertOne);
            return CreatePersonResult.Create(insertOne.Value.Id);
        }

        public async Task<bool> Exists(Guid id) { return await Query.Exists(a => a.Id == id); }

        public async Task<Person> Get(Guid id) { return await Query.FindById(id); }

        public async Task<UpdateResult> PatchAttribute(ICallistoUpdate<Person> update)
        {
            IClientSessionHandle session = await StartSessionAsync(update);
            UpdateResult result = await Update.One(update);
            await session.CommitTransactionAsync();
            EndSession(update);
            return result;
        }
    }
}
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Results;
using Solari.Titan;

namespace Solari.Samples.Infrastructure
{
    public class PersonRepository : CallistoRepository<Person>, IPersonRepository
    {
        private readonly ILogger<PersonRepository> _logger;
        public PersonRepository(ICallistoContext<Person> context, ILogger<PersonRepository> logger) : base(context) { _logger = logger; }

        public async Task<CreatePersonResult> InsertPerson(ICallistoInsert<Person> insert)
        {
            _logger.LogInformation("Executing database insertion");
            await Insert.One(insert);
            return CreatePersonResult.Create(insert.Value.Id);
        }

        public async Task<bool> Exists(ObjectId id) { return await Query.Exists(a => a.Id == id); }

        public async Task<Person> Get(ObjectId id) { return await Query.FindById(id); }

        public async Task<UpdateResult> PatchAttribute(ICallistoUpdate<Person> update)
        {
            IClientSessionHandle session = await StartSessionAsync();
            session.StartTransaction();
            update.AddSessionHandle(session);
            UpdateResult result = await Update.One(update);
            await session.CommitTransactionAsync();
            EndSession(session);
            return result;
        }
    }
}


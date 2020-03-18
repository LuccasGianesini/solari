using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Exceptions;
using Solari.Samples.Domain.Person.Results;
using Solari.Titan;
using Solari.Vanth;

namespace Solari.Samples.Infrastructure
{
    public class PersonRepository : CallistoRepository<Person>, IPersonRepository
    {
        private readonly ITitanLogger<PersonRepository> _logger;
        public PersonRepository(ICallistoContext context, ITitanLogger<PersonRepository> logger) : base(context) { _logger = logger; }

        public async Task<CreatePersonResult> InsertPerson(ICallistoInsert<Person> insert)
        {
            _logger.Information("Executing database insertion");
            await Insert.One(insert);
            return CreatePersonResult.Create(insert.Value.Id);
        }

        public async Task<bool> Exists(ObjectId id) { return await Query.Exists(a => a.Id == id); }

        public async Task<Person> Get(ObjectId id) => await Query.FindById(id);

        public async Task<UpdateResult> PatchAttribute(ICallistoUpdate<Person> update)
        {
            using (IClientSessionHandle session = await Context.Connection.GetClient().StartSessionAsync())
            {
                session.StartTransaction();
                update.AddSessionHandle(session);
                UpdateResult result = await Update.One(update);
                await session.CommitTransactionAsync();
                return result;
            }
        }
    }
}
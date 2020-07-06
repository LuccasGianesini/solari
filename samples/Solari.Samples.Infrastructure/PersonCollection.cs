using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Solari.Callisto;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Framework;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Results;

namespace Solari.Samples.Infrastructure
{
    public class PersonCollection : CallistoCollection<Person>, IPersonCollection
    {
        public PersonCollection(ICallistoCollectionContext<Person> collectionContext, ICollectionOperators<Person> operators)
            : base(collectionContext, operators)
        {
        }

        public async Task<CreatePersonResult> InsertPerson(ICallistoInsertOne<Person> insertOne)
        {
            await Operators.Insert.One(insertOne);
            return CreatePersonResult.Create(insertOne.Value.Id);
        }

        public async Task<bool> Exists(Guid id)
        {
            return await Operators.Query.Exists(a => a.Id == id);
        }

        public async Task<Person> Get(Guid id)
        {
            return await Operators.Query.FindById(id);
        }

        public async Task<UpdateResult> PatchAttribute(ICallistoUpdate<Person> update)
        {
            IClientSessionHandle session = await StartSessionAsync(update);
            UpdateResult result = await Operators.Update.One(update);
            await session.CommitTransactionAsync();
            EndSession(update);
            return result;
        }
    }
}

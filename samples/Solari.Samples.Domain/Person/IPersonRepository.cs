using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person.Results;

namespace Solari.Samples.Domain.Person
{
    public interface IPersonRepository
    {
        Task<CreatePersonResult> InsertPerson(ICallistoInsertOne<Person> insertOne);
        Task<UpdateResult> PatchAttribute(ICallistoUpdate<Person> update);
        Task<bool> Exists(ObjectId id);
        Task<Person> Get(ObjectId id);
    }
}
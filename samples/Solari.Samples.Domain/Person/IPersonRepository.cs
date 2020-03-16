using System.Threading.Tasks;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person.Results;
using Solari.Vanth;

namespace Solari.Samples.Domain.Person
{
    public interface IPersonRepository
    {
        Task<CreatePersonResult> InsertPerson(ICallistoInsert<Person> insert);
        Task<UpdateResult> AddAttribute(ICallistoUpdate<Person> update);
    }
}
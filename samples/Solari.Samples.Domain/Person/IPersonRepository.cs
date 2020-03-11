using System.Threading.Tasks;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person.Dtos;
using Solari.Vanth;

namespace Solari.Samples.Domain.Person
{
    public interface IPersonRepository
    {
        Task<CommonResponse<PersonInsertedDto>> InsertPerson(ICallistoInsert<Person> insert);
        Task<CommonResponse<UpdateResult>> AddAttribute(ICallistoUpdate<Person> update);
    }
}
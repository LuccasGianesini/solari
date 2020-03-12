using System.Threading.Tasks;
using Solari.Samples.Domain.Person.Dtos;
using Solari.Samples.Domain.Person.Results;
using Solari.Vanth;

namespace Solari.Samples.Domain.Person
{
    public interface IPersonApplication
    {
        Task<InsertPersonResult> InsertPerson(InsertPersonDto dto);
        Task<long> AddAttributeToPerson(PersonAddAttributeDto dto);
    }
}
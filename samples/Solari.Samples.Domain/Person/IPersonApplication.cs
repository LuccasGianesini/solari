using System.Threading.Tasks;
using Solari.Samples.Domain.Person.Dtos;
using Solari.Vanth;

namespace Solari.Samples.Domain.Person
{
    public interface IPersonApplication
    {
        Task<CommonResponse<PersonInsertedDto>> InsertPerson(InsertPersonDto dto);
        Task<CommonResponse<long>> AddAttributeToPerson(PersonAddAttributeDto dto);
    }
}
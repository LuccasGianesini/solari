using System.Threading.Tasks;
using Solari.Samples.Domain.Person.Commands;
using Solari.Samples.Domain.Person.Results;
using Solari.Vanth;

namespace Solari.Samples.Domain.Person
{
    public interface IPersonApplication
    {
        Task<CreatePersonResult> InsertPerson(CreatePersonCommand command);
        // Task<SimpleResult<long>> AddAttributeToPerson(PersonAttributeCommand command);
    }
}

using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person.Commands;

namespace Solari.Samples.Domain.Person
{
    public interface IPersonOperations
    {
        ICallistoInsert<Person> CreateInsertOperation(CreatePersonCommand createPersonCommand);
        ICallistoUpdate<Person> CreateAddAttributeOperation(PersonAttributeCommand command);
        ICallistoUpdate<Person> CreateUpdateAttributeOperation(PersonAttributeCommand command);
    }
}
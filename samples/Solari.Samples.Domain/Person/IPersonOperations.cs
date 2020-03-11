using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person.Dtos;

namespace Solari.Samples.Domain.Person
{
    public interface IPersonOperations
    {
        ICallistoInsert<Person> CreateInsertOperation(InsertPersonDto insertPersonDto);
        ICallistoUpdate<Person> CreateAddAttributeOperation(PersonAddAttributeDto dto);
    }
}
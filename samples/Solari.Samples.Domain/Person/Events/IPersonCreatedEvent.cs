using Solari.Eris;
using Solari.Samples.Domain.Person.Results;

namespace Solari.Samples.Domain.Person.Events
{
    public interface IPersonCreatedEvent
    {
        CreatePersonResult Result { get; }
    }
}
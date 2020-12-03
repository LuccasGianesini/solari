using System.Collections.Generic;
using Solari.Samples.Domain.Person.Dtos;

namespace Solari.Samples.Domain.Person.Commands
{
    public class PersonAttributeCommand
    {
        public string PersonId { get; set; }
        public List<PersonAttributeDto> Values { get; set; }
        // public SimpleResult<object> SimpleResult { get; set; }
    }
}

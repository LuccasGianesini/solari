using System.Collections.Generic;
using Solari.Samples.Domain.Person.Dtos;

namespace Solari.Samples.Domain.Person.Dtos
{
    public class InsertPersonDto
    {
        public string Name { get; set; }
        public List<AddPersonAddAttributeDto> Attributes { get; set; }
    }
}
using System.Collections.Generic;
using Solari.Samples.Domain.Person.Commands;

namespace Solari.Samples.Domain.Person.Dtos
{
    public class CreatePersonRestIn
    {
        public string Name { get; set; }
        public List<AddPersonAttributeCommand> Attributes { get; set; }
    }
}
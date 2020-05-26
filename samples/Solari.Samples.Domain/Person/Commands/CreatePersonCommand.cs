using System.Collections.Generic;
using Solari.Eris;
using Solari.Samples.Domain.Person.Dtos;

namespace Solari.Samples.Domain.Person.Commands
{
    public class CreatePersonCommand : ICommand
    {
        public string Name { get; set; }
        public List<PersonAttributeDto> Attributes { get; set; }
    }
}
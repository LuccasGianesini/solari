using System.Collections.Generic;
using Solari.Eris;
using Solari.Samples.Domain.Person.Dtos;
using Solari.Samples.Domain.Person.Results;

namespace Solari.Samples.Domain.Person.Commands
{
    public class CreatePersonCommand : ICommand
    {
        public CreatePersonCommand(string name, List<PersonAttributeDto> attributes)
        {
            Name = name;
            Attributes = attributes;
        }

        public CreatePersonCommand() { }

        public string Name { get; set; }
        public List<PersonAttributeDto> Attributes { get; set; }

        public CreatePersonResult Result { get; set; }
    }
}
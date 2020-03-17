using System.Collections.Generic;
using Convey.CQRS.Commands;
using Microsoft.AspNetCore.Mvc;
using Solari.Samples.Domain.Person.Results;

namespace Solari.Samples.Domain.Person.Commands
{
    public class CreatePersonCommand : ICommand
    {
        public CreatePersonCommand(string name, List<PersonAttributeCommand> attributes)
        {
            Name = name;
            Attributes = attributes;
        }

        public CreatePersonCommand()
        {
            
        }
        public string Name { get; set; }
        public List<PersonAttributeCommand> Attributes { get; set; }
        
        public CreatePersonResult Result { get; set; }

        
    }
}
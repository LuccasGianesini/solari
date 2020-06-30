using System.Collections.Generic;
using MongoDB.Bson;
using Solari.Eris;
using Solari.Samples.Domain.Person.Dtos;
using Solari.Vanth;

namespace Solari.Samples.Domain.Person.Commands
{
    public class PersonAttributeCommand : ICommand
    {
        public string PersonId { get; set; }
        public List<PersonAttributeDto> Values { get; set; }
        public Result<object> Result { get; set; }
    }
}
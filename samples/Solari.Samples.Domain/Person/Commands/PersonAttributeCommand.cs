﻿using System.Collections.Generic;
using MongoDB.Bson;
using Solari.Eris;
using Solari.Samples.Domain.Person.Dtos;
using Solari.Vanth;

namespace Solari.Samples.Domain.Person.Commands
{
    public class PersonAttributeCommand : ICommand
    {
        public string PersonId { get; set; }
        public PatchOperation Operation { get; set; }
        public List<PersonAttributeDto> Values { get; set; }
        
        public CommonResponse<object> Result { get; set; }
        
        public ObjectId ObjectId => MongoDB.Bson.ObjectId.Parse(PersonId);
        
    }
}
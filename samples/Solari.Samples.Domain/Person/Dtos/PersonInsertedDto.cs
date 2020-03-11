using System;
using MongoDB.Bson;

namespace Solari.Samples.Domain.Person.Dtos
{
    public class PersonInsertedDto
    {
        public PersonInsertedDto(bool success, string id)
        {
            Success = success;
            Id = id;
        }

        public bool Success { get; }
        public string Id { get; }

        public static PersonInsertedDto Create(ObjectId objectId)
        {
            var id = objectId.ToString(); 
            return id.Equals(Callisto.Abstractions.CallistoConstants.ObjectIdDefaultValueAsString)
                       ? new PersonInsertedDto(false, string.Empty)
                       : new PersonInsertedDto(true, id);
        }
    }
}
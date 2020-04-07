using System;
using MongoDB.Bson;
using Solari.Titan;

namespace Solari.Samples.Domain.Person.Results
{
    public class CreatePersonResult
    {
        public CreatePersonResult(bool success, string id)
        {
            Success = success;
            Id = id;
        }

        public bool Success { get; }
        public string Id { get; }

        public static CreatePersonResult Create(ObjectId objectId)
        {
            var id = objectId.ToString(); 
            return id.Equals(Callisto.Abstractions.CallistoConstants.ObjectIdDefaultValueAsString)
                       ? new CreatePersonResult(false, string.Empty)
                       : new CreatePersonResult(true, id);
        }
    }
}
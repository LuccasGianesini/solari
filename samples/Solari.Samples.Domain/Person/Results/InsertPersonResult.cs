using MongoDB.Bson;

namespace Solari.Samples.Domain.Person.Results
{
    public class InsertPersonResult
    {
        public InsertPersonResult(bool success, string id)
        {
            Success = success;
            Id = id;
        }

        public bool Success { get; }
        public string Id { get; }

        public static InsertPersonResult Create(ObjectId objectId)
        {
            var id = objectId.ToString(); 
            return id.Equals(Callisto.Abstractions.CallistoConstants.ObjectIdDefaultValueAsString)
                       ? new InsertPersonResult(false, string.Empty)
                       : new InsertPersonResult(true, id);
        }
    }
}
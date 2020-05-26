using System;

namespace Solari.Samples.Domain.Person.Results
{
    public class CreatePersonResult
    {
        public CreatePersonResult(bool success, Guid id)
        {
            Success = success;
            Id = id;
        }

        public bool Success { get; }
        public Guid Id { get; }

        public static CreatePersonResult Create(Guid objectId)
        {
            return objectId == Guid.Empty
                       ? new CreatePersonResult(false, Guid.Empty)
                       : new CreatePersonResult(true, objectId);
        }
    }
}
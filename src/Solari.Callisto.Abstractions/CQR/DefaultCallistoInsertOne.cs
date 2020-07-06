using System.Threading;
using FluentValidation.Results;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Abstractions.Validators;

namespace Solari.Callisto.Abstractions.CQR
{
    public class DefaultCallistoInsertOne<T> : ICallistoInsertOne<T> where T : class, IDocumentRoot
    {
        public DefaultCallistoInsertOne(string operationName, T value, InsertOneOptions insertOneOptions)
        {
            OperationName = operationName;
            Value = value;
            InsertOneOptions = insertOneOptions;
        }

        public string OperationName { get; }
        public CancellationToken CancellationToken { get; set; }
        public T Value { get; }
        public IClientSessionHandle ClientSessionHandle { get; set; }

        public void Validate()
        {
            new InsertOneValidator<T>().ValidateCallistoOperation(this);
        }

        public InsertOneOptions InsertOneOptions { get; }
    }
}
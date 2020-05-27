using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentValidation.Results;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Abstractions.Validators;

namespace Solari.Callisto.Abstractions.CQR
{
    public class DefaultCallistoInsertMany<T> : ICallistoInsertMany<T> where T : class, IDocumentRoot
    {
        public DefaultCallistoInsertMany(string operationName, IEnumerable<T> values, InsertManyOptions insertManyOptions)
        {
            OperationName = operationName;
            Values = values;
            InsertManyOptions = insertManyOptions;
        }

        public string OperationName { get; }
        public CancellationToken CancellationToken { get; set; }
        public IEnumerable<T> Values { get; }
        public InsertManyOptions InsertManyOptions { get; }
        public IClientSessionHandle ClientSessionHandle { get; set; }

        public void Validate()
        {
            new InsertManyValidator<T>().ValidateCallistoOperation(this);
        }
    }
}
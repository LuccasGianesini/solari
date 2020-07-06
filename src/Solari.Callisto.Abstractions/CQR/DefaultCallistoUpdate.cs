using System.Threading;
using FluentValidation.Results;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Abstractions.Validators;

namespace Solari.Callisto.Abstractions.CQR
{
    public sealed class DefaultCallistoUpdate<T> : ICallistoUpdate<T> where T : class, IDocumentRoot
    {
        public DefaultCallistoUpdate(string operationName, UpdateDefinition<T> updateDefinition,
                                     FilterDefinition<T> filterDefinition, UpdateOptions updateOptions)
        {
            OperationName = operationName;
            UpdateDefinition = updateDefinition;
            FilterDefinition = filterDefinition;
            UpdateOptions = updateOptions;
        }

        public string OperationName { get; }
        public CancellationToken CancellationToken { get; set; }
        public UpdateDefinition<T> UpdateDefinition { get; }
        public FilterDefinition<T> FilterDefinition { get; }
        public UpdateOptions UpdateOptions { get; }
        public IClientSessionHandle ClientSessionHandle { get; set; }

        public void Validate()
        {
            new UpdateValidator<T>().ValidateCallistoOperation(this);
        }
    }
}
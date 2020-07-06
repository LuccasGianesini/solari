using System.Threading;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Abstractions.Validators;

namespace Solari.Callisto.Abstractions.CQR
{
    public class DefaultCallistoDelete<T> : ICallistoDelete<T> where T : class, IDocumentRoot
    {
        public DefaultCallistoDelete(string operationName, FilterDefinition<T> filterDefinition, DeleteOptions deleteOptions)
        {
            OperationName = operationName;
            FilterDefinition = filterDefinition;
            DeleteOptions = deleteOptions;
        }

        public string OperationName { get; }
        public CancellationToken CancellationToken { get; set; }
        public IClientSessionHandle ClientSessionHandle { get; set; }

        public void Validate()
        {
            new DeleteValidator<T>().ValidateCallistoOperation(this);
        }

        public FilterDefinition<T> FilterDefinition { get; }
        public DeleteOptions DeleteOptions { get; }
    }
}
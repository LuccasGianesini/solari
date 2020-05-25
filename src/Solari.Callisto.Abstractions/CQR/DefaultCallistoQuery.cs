using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Abstractions.Validators;

namespace Solari.Callisto.Abstractions.CQR
{
    public class DefaultCallistoQuery<T, TResult> : ICallistoQuery<T, TResult> where T : class, IDocumentRoot
    {
        public DefaultCallistoQuery(string operationName, FilterDefinition<T> filterDefinition, 
                                    Func<IAsyncCursor<T>, TResult> resultFunction, FindOptions<T> findOptions)
        {
            OperationName = operationName;
            FindOptions = findOptions;
            FilterDefinition = filterDefinition;
            ResultFunction = resultFunction;
        }

        public string OperationName { get; }
        public CancellationToken CancellationToken { get; set; }
        public IClientSessionHandle ClientSessionHandle { get;  set; }

        public void Validate()
        {
            new QueryValidator<T, TResult>().ValidateCallistoOperation(this);
        }

        public FindOptions<T> FindOptions { get; }
        public FilterDefinition<T> FilterDefinition { get; }
        public Func<IAsyncCursor<T>, TResult> ResultFunction { get; }
    }
}
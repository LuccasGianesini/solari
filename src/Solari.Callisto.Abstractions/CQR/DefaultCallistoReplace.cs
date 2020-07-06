using System;
using System.Threading;
using FluentValidation.Results;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Abstractions.Validators;

namespace Solari.Callisto.Abstractions.CQR
{
    public class DefaultCallistoReplace<T> : ICallistoReplace<T> where T : class, IDocumentRoot
    {
        public DefaultCallistoReplace(string operationName, T replacement, 
                                      FilterDefinition<T> filterDefinition, ReplaceOptions replaceOptions)
        {
            OperationName = operationName;
            Replacement = replacement;
            FilterDefinition = filterDefinition;
            ReplaceOptions = replaceOptions;
        }
        public string OperationName { get; }
        public CancellationToken CancellationToken { get; set; }
        public IClientSessionHandle ClientSessionHandle { get;  set; }

        public void Validate()
        {
            new ReplaceValidator<T>().ValidateCallistoOperation(this);
        }

        public T Replacement { get; }
        public FilterDefinition<T> FilterDefinition { get; }
        public ReplaceOptions ReplaceOptions { get; }
        }
}
using System;
using System.Threading;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Abstractions.Validators;

namespace Solari.Callisto.Abstractions.CQR
{
    public class DefaultCallistoAggregation<T, TProjectionModel, TResult> : ICallistoAggregation<T, TProjectionModel, TResult> 
        where T : class, IDocumentRoot
        where TProjectionModel: class
    {
        public DefaultCallistoAggregation(string operationName, Func<IAsyncCursor<TProjectionModel>, TResult> resultFunction,
                                          PipelineDefinition<T, TProjectionModel> pipelineDefinition, AggregateOptions aggregateOptions)
        {
            OperationName = operationName;
            ResultFunction = resultFunction;
            PipelineDefinition = pipelineDefinition;
            AggregateOptions = aggregateOptions;
        }

        public string OperationName { get; }
        public CancellationToken CancellationToken { get; set; }
        public IClientSessionHandle ClientSessionHandle { get;  set; }
        public void Validate()
        {
            new AggregationValidator<T, TProjectionModel, TResult>().ValidateCallistoOperation(this);
        }

        public Func<IAsyncCursor<TProjectionModel>, TResult> ResultFunction { get; }
        public PipelineDefinition<T, TProjectionModel> PipelineDefinition { get; }
        public AggregateOptions AggregateOptions { get; }
    }
}
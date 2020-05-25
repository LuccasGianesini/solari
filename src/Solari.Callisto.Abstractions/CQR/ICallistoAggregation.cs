using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Solari.Callisto.Abstractions.CQR
{
    public interface ICallistoAggregation<T, TProjectionModel, out TResult> : ICallistoOperation<T> 
        where T : class, IDocumentRoot
        where TProjectionModel: class
    {
        Func<IAsyncCursor<TProjectionModel>, TResult> ResultFunction { get; }
        PipelineDefinition<T, TProjectionModel> PipelineDefinition { get; }
        AggregateOptions AggregateOptions { get; }
    }
}
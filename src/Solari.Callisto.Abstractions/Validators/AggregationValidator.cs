using System;
using FluentValidation;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto.Abstractions.Validators
{
    public class AggregationValidator<T, TProjectionModel, TResult> : AbstractValidator<ICallistoAggregation<T, TProjectionModel, TResult>> 
        where T : class, IDocumentRoot
        where TProjectionModel: class
    {
        public AggregationValidator()
        {
            RuleFor(a => a)
                .NotNull()
                .WithMessage($"Cannot invoke a null instance of a {nameof(ICallistoAggregation<T, TProjectionModel, TResult>)} operation")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
            RuleFor(a => a.PipelineDefinition)
                .NotEmpty()
                .WithMessage($"An {nameof(ICallistoAggregation<T, TProjectionModel, TResult>)} requires an {nameof(PipelineDefinition<T, TProjectionModel>)}")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
            RuleFor(a => a.ResultFunction)
                .NotNull()
                .WithMessage($"An {nameof(ICallistoQuery<T, TResult>)} requires an {nameof(Func<IAsyncCursor<TProjectionModel>, TResult>)} result function.")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
        }
    }
}
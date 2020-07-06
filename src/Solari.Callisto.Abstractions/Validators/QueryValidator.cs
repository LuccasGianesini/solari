using System;
using FluentValidation;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto.Abstractions.Validators
{
    public class QueryValidator<T, TResult> : AbstractValidator<ICallistoQuery<T, TResult>> where T: class, IDocumentRoot
    {
        public QueryValidator()
        {
            RuleFor(a => a)
                .NotNull()
                .WithMessage($"Cannot invoke a null instance of a {nameof(ICallistoQuery<T, TResult>)} operation")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
            RuleFor(a => a.FilterDefinition)
                .NotEmpty()
                .WithMessage($"An {nameof(ICallistoQuery<T, TResult>)} requires an {nameof(FilterDefinition<T>)}")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
            RuleFor(a => a.ResultFunction)
                .NotNull()
                .WithMessage($"An {nameof(ICallistoQuery<T, TResult>)} requires an {nameof(Func<IAsyncCursor<T>, TResult>)} result function.")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
        }
    }
}
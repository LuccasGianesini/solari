using FluentValidation;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Contracts.CQR;

namespace Solari.Callisto.Abstractions.Validators
{
    public class QueryValidator<T> : AbstractValidator<ICallistoQuery<T>> where T : class, IDocumentRoot
    {
        public QueryValidator()
        {
            RuleFor(a => a)
                .NotNull()
                .WithMessage($"Cannot invoke a null instance of a {nameof(ICallistoQuery<T>)} operation")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
            RuleFor(a => a.FilterDefinition)
                .NotEmpty()
                .WithMessage($"An {nameof(ICallistoQuery<T>)} requires an {nameof(FilterDefinition<T>)}")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
        }
    }
}

using FluentValidation;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto.Abstractions.Validators
{
    public class ReplaceValidator<T> : AbstractValidator<ICallistoReplace<T>> where T: class, IDocumentRoot
    {
        public ReplaceValidator()
        {
            RuleFor(a => a)
                .NotNull()
                .WithMessage($"Cannot invoke a null instance of a {nameof(ICallistoReplace<T>)} operation")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
            RuleFor(a => a.FilterDefinition)
                .NotEmpty()
                .WithMessage($"An {nameof(ICallistoReplace<T>)} requires an {nameof(FilterDefinition<T>)}")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
            RuleFor(a => a.Replacement)
                .NotEmpty()
                .WithMessage($"An {nameof(ICallistoReplace<T>)} requires an replacement value")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
        }
    }
}
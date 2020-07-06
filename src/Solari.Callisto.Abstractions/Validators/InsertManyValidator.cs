using FluentValidation;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto.Abstractions.Validators
{
    public class InsertManyValidator<T> : AbstractValidator<ICallistoInsertMany<T>> where T : class, IDocumentRoot
    {
        public InsertManyValidator()
        {
            RuleFor(a => a)
                .NotNull()
                .WithMessage($"Cannot invoke a null instance of a {nameof(ICallistoInsertOne<T>)} operation")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
            RuleFor(a => a.Values)
                .NotEmpty()
                .WithMessage($"An {nameof(ICallistoInsertMany<T>)} operation requires a value")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
        }
    }
}
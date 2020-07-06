using FluentValidation;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto.Abstractions.Validators
{
    public class InsertOneValidator<T> : AbstractValidator<ICallistoInsertOne<T>> where T : class, IDocumentRoot
    {
        public InsertOneValidator()
        {
            RuleFor(a => a)
                .NotNull()
                .WithMessage($"Cannot invoke a null instance of a {nameof(ICallistoInsertOne<T>)} operation")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
            RuleFor(a => a.Value)
                .NotNull()
                .WithMessage($"An {nameof(ICallistoInsertOne<T>)} operation requires a value")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
        }
    }
}
using System.Linq;
using FluentValidation;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.CQR;

namespace Solari.Callisto.Abstractions.Validators
{
    public class UpdateValidator<T> : AbstractValidator<ICallistoUpdate<T>> where T : class, IDocumentRoot
    {
        public UpdateValidator()
        {
            RuleFor(a => a)
                .NotNull()
                .WithMessage($"Cannot invoke a null instance of a {nameof(ICallistoUpdate<T>)} operation")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
            RuleFor(a => a.FilterDefinition)
                .NotEmpty()
                .WithMessage($"An {nameof(ICallistoUpdate<T>)} requires an {nameof(FilterDefinition<T>)}")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);

            RuleFor(a => a.UpdateDefinition)
                .NotEmpty()
                .WithMessage($"An {nameof(ICallistoUpdate<T>)} requires an {nameof(FilterDefinition<T>)}")
                .WithName(a => a.OperationName)
                .WithSeverity(Severity.Error);
        }
    }
}
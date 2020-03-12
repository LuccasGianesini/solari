using FluentValidation;
using Solari.Samples.Domain.Person.Dtos;

namespace Solari.Samples.Domain.Person.Validators
{
    public class InsertPersonDtoValidator : AbstractValidator<InsertPersonDto>
    {
        public InsertPersonDtoValidator()
        {
            RuleFor(a => a).NotNull()
                           .WithErrorCode(Constants.NullObjectErrorCode)
                           .WithName(Constants.NullObjectErrorName)
                           .WithMessage("Provided object cannot be null!");

            RuleFor(a => a.Name).NotEmpty()
                                .WithErrorCode(Constants.EmptyPropertyErrorCode)
                                .WithName(Constants.EmptyPropertyName)
                                .WithMessage("Person name cannot be empty!")
                                .NotNull()
                                .WithErrorCode(Constants.NullPropertyErrorCode)
                                .WithName(Constants.NullObjectErrorName)
                                .WithMessage("Person name cannot be empty!");
        }

    }
}
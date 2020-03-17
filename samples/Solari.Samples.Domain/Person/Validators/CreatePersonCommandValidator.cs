using FluentValidation;
using Solari.Samples.Domain.Person.Commands;
using Solari.Vanth.Validation;

namespace Solari.Samples.Domain.Person.Validators
{
    public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
    {
        public CreatePersonCommandValidator()
        {
            
            RuleFor(a => a).NotNull()
                           .WithErrorCode(Constants.NullObjectErrorCode)
                           .WithName(Constants.NullObjectErrorName)
                           .WithMessage("Provided object cannot be null!");

            RuleFor(a => a.Name).NotEmpty()
                                .WithErrorCode(Constants.EmptyPropertyErrorCode)
                                .WithName(Constants.EmptyPropertyErrorName)
                                .WithMessage("Person name cannot be empty!");
        }

        
    }
}
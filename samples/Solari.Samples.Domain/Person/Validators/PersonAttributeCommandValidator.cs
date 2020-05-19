using FluentValidation;
using Solari.Callisto.Abstractions;
using Solari.Samples.Domain.Person.Commands;

namespace Solari.Samples.Domain.Person.Validators
{
    public class AddPersonAttributeDtoValidator : AbstractValidator<PersonAttributeCommand>
    {
        public AddPersonAttributeDtoValidator()
        {
            RuleFor(a => a.PersonId)
                .NotEmpty()
                .WithMessage("PersonId is required")
                .WithErrorCode(Constants.EmptyPropertyErrorCode)
                .WithName(Constants.EmptyPropertyErrorName)
                .NotEqual(CallistoConstants.ObjectIdDefaultValueAsString)
                .WithMessage("The provided PersonId is invalid")
                .WithErrorCode(Constants.InvalidObjectIdErrorCode)
                .WithName(Constants.InvalidObjectIdErrorName);
            RuleFor(a => a.Values)
                .NotEmpty()
                .WithMessage("The values collection cannot be empty")
                .WithErrorCode(Constants.EmptyPropertyErrorCode)
                .WithName(Constants.EmptyPropertyErrorName);
            RuleFor(a => a.Operation)
                .NotEmpty()
                .WithMessage("The patch operation cannot be empty or null")
                .WithErrorCode(Constants.EmptyPropertyErrorCode)
                .WithName(Constants.EmptyPropertyErrorName)
                .Equal(PatchOperation.Add)
                .Equal(PatchOperation.Remove)
                .Equal(PatchOperation.Update)
                .WithMessage("Operation must be 'Add' or 'Remove' or 'Update'")
                .WithErrorCode("ERR-023")
                .WithName("Invalid operation name");
        }
    }
}
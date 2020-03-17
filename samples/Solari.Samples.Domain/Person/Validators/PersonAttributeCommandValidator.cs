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

            RuleFor(a => a.AttributeName)
                .NotEmpty()
                .WithMessage("AttributeValue cannot be empty")
                .WithErrorCode(Constants.EmptyPropertyErrorCode)
                .WithName(Constants.EmptyPropertyErrorName);
            RuleFor(a => a.AttributeValue)
                .NotEmpty()
                .WithMessage("AttributeValue cannot be empty")
                .WithErrorCode(Constants.EmptyPropertyErrorCode)
                .WithName(Constants.EmptyPropertyErrorName);
        }
        
    }
}
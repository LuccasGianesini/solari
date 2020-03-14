using FluentValidation;
using Solari.Samples.Domain.Person.Dtos;

namespace Solari.Samples.Domain.Person.Validators
{
    public class AddPersonAttributeDtoValidator : AbstractValidator<AddPersonAddAttributeDto>
    {
        public AddPersonAttributeDtoValidator()
        {
            RuleFor(a => a.AttributeName)
                .NotEmpty()
                .WithMessage("AttributeValue cannot be empty")
                .WithErrorCode(Constants.EmptyPropertyName)
                .WithName(Constants.EmptyPropertyName);
            RuleFor(a => a.AttributeValue)
                .NotEmpty()
                .WithMessage("AttributeValue cannot be empty")
                .WithErrorCode(Constants.EmptyPropertyName)
                .WithName(Constants.EmptyPropertyName);
        }
        
    }
}
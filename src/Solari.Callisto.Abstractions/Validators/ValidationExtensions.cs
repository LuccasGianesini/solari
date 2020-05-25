using System.Linq;
using System.Text;
using FluentValidation;
using FluentValidation.Results;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Abstractions.Validators
{
    internal static class ValidationExtensions
    {
        internal static void ValidateCallistoOperation<T>(this AbstractValidator<T> validator, T instance)
        {
            if (validator is null)
                throw new CallistoException("Cannot call an null validator instance");
            ValidationResult results = validator.Validate(instance);
            if (results.IsValid)
                return;
            Throw(results);
        }

        private static void Throw(ValidationResult result)
        {
            StringBuilder exceptionMessage = new StringBuilder().Append("The provided operation did not appears to be valid.").AppendLine()
                                                                .Append("Errors: ").AppendLine();
            exceptionMessage = ExtractErrors(result, exceptionMessage);
            throw new CallistoException(exceptionMessage.ToString());
        }

        private static StringBuilder ExtractErrors(ValidationResult result, StringBuilder stringBuilder)
        {
            foreach (ValidationFailure validationFailure in result.Errors)
            {
                stringBuilder.Append(validationFailure.ErrorMessage).AppendLine();
            }

            return stringBuilder;
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Solari.Vanth.Builders;

namespace Solari.Vanth.Validation
{
    public class VanthValidatorAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.Count == 0)
            {
                await next();
            }
            else
            {
                var validatorFactory = context.HttpContext.RequestServices.GetRequiredService<IValidatorFactory>();
                var response = new CommonResponse<object>();
                foreach ((string key, object value) in context.ActionArguments)
                {
                    if (value == null) continue;
                    IValidator validator = validatorFactory.GetValidator(value.GetType());
                    if (validator == null) continue;
                    ValidationResult result = validator.Validate(value);
                    if (!result.Errors.Any()) continue;
                    CommonErrorResponse error = new CommonErrorResponseBuilder()
                                                .WithCode(CommonErrorCode.ValidationErrorCode)
                                                .WithErrorType(CommonErrorType.ValidationError)
                                                .WithMessage("Invalid Model State!")
                                                .WithTarget($"Action: {context.ActionDescriptor.DisplayName} ActionArgument: {key}")
                                                .Build();

                    foreach (ValidationFailure failure in result.Errors)
                        error.AddDetailedError(builder => builder.WithErrorCode(failure.ErrorCode)
                                                                 .WithMessage(failure.ErrorMessage)
                                                                 .WithTarget(failure.PropertyName)
                                                                 .Build());

                    response.AddError(error);
                }

                if (!response.HasErrors)
                {
                    await next();
                }
                else
                {
                    context.Result = new BadRequestObjectResult(response);
                }
            }
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Solari.Vanth.Builders;
using Solari.Vanth.Extensions;

namespace Solari.Vanth.Validation
{
    public class VanthValidatorAttribute : Attribute, IAsyncActionFilter
    {
        //TODO FIX
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.Count == 0)
            {
                await next();
            }
            else
            {
                var validatorFactory = context.HttpContext.RequestServices.GetRequiredService<IValidatorFactory>();
                // var resultFactory = context.HttpContext.RequestServices.GetRequiredService<IResultFactory>();
                // ISimpleResult<object> response = new SimpleResult<object>();
                // foreach ((string key, object value) in context.ActionArguments)
                // {
                //     if (value == null) continue;
                //     IValidator validator = validatorFactory.GetValidator(value.GetType());
                //     if (validator == null) continue;
                //     ValidationResult result = await validator.ValidateAsync(new ValidationContext<object>(value));
                //     if (result.IsValid || !result.Errors.Any()) continue;
                //     response = resultFactory.ValidationError<object>(result, $"Action: {context.ActionDescriptor.DisplayName} ActionArgument: {key}");
                // }
                //
                // if (!response.HasErrors())
                // {
                //     await next();
                // }
                // else
                // {
                //     context.Result = new BadRequestObjectResult(response);
                // }
            }
        }
    }
}

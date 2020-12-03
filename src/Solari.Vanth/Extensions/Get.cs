using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Solari.Vanth.Extensions
{
    public static partial class ResultExtensions
    {
        public static IDictionary<string, string> ExtractValidationErrors(this ValidationResult validationResult)
        {
            if (validationResult.IsValid)
                return null;
            return validationResult.Errors.ToDictionary(key => key.PropertyName, value => value.ErrorMessage);
        }

        public static bool TryGetDetails(this IError error, out List<IErrorDetail> details)
        {
            if (error.HasDetails())
            {
                details = error.Details;
                return true;
            }

            details = new List<IErrorDetail>();
            return false;
        }

        // TODO FIX
        // public static bool TryGetData<T>(this ISimpleResult<T> simpleResult, out T data)
        // {
        //     if (simpleResult.HasData())
        //     {
        //         data = simpleResult.Data;
        //         return true;
        //     }
        //
        //     data = default;
        //     return false;
        // }
        //
        // public static bool TryGetErrors<T>(this ISimpleResult<T> simpleResult, out List<IError> errors)
        // {
        //     if (simpleResult.HasErrors())
        //     {
        //         errors = simpleResult.Errors;
        //         return true;
        //     }
        //
        //     errors = null;
        //     return false;
        // }


    }
}

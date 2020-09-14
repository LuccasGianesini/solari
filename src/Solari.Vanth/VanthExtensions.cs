using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using Solari.Sol.Utils;

namespace Solari.Vanth
{
    public static class VanthExtensions
    {
        public static bool HasData<T>(this Result<T> result)
        {
            if (result is null)
                return false;
            if (result.Data != null)
                return true;
            return false;
        }

        public static bool HasErrors<T>(this IResult<T> result)
        {
            if (result is null)
                return false;
            if (result.Errors.Any())
                return true;
            return false;
        }

        public static bool HasDetails(this IError error)
        {
            if (error is null)
                return false;
            if (error.Details.Any())
                return true;
            return false;
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

        public static bool TryGetData<T>(this IResult<T> result, out T data)
        {
            if (result.HasData())
            {
                data = result.Data;
                return true;
            }
            data = default;
            return false;
        }

        public static bool TryGetErrors<T>(this IResult<T> result, out List<IError> errors)
        {
            if (result.HasErrors())
            {
                errors = result.Errors;
                return true;
            }
            errors = null;
            return false;
        }
    }
}

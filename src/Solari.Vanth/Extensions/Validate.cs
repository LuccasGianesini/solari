using System.Linq;

namespace Solari.Vanth.Extensions
{
    //TODO Fix
    public static partial class ResultExtensions
    {
        // public static bool IsSuccessStatusCode<TData>(this ISimpleResult<TData> simpleResult)
        // {
        //     return simpleResult.StatusCode >= 200 && simpleResult.StatusCode < 300;
        // }

        // public static bool IsSuccess<T>(this ISimpleResult<T> simpleResult)
        // {
        //     return simpleResult.HasData() || simpleResult.Success || simpleResult.IsSuccessStatusCode();
        // }
        // public static bool IsFailure<T>(this ISimpleResult<T> simpleResult)
        // {
        //     return simpleResult.Errors.Any();
        // }
        // public static bool HasData<T>(this ISimpleResult<T> simpleResult)
        // {
        //     if (simpleResult is null)
        //         return false;
        //     if (simpleResult.Data == null)
        //         return false;
        //
        //     return true;
        // }
        //
        // public static bool HasErrors<T>(this ISimpleResult<T> simpleResult)
        // {
        //     if (simpleResult is null)
        //         return false;
        //     if (!simpleResult.Errors.Any())
        //         return false;
        //     return true;
        // }

        public static bool HasDetails(this IError error)
        {
            if (error is null)
                return false;
            if (!error.Details.Any())
                return false;
            return true;
        }

    }
}

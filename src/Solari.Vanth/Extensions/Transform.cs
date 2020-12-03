using System;
using System.Linq;
using Newtonsoft.Json;
using Solari.Sol.Abstractions;
using Solari.Vanth.Builders;

namespace Solari.Vanth.Extensions
{
    //TODO FIX
    public static partial class ResultExtensions
    {
        /// <summary>
        ///     Creates a new <see cref="SimpleResult{TData}" /> with a different generic type. And add the new generic type value.
        /// </summary>
        /// <param name="simpleResult">CommonResponse object to be cloned.</param>
        /// <param name="newTypeValue">The new generic type value. Value must not be null.</param>
        /// <param name="addErrors">Indicates if the errors present in the old CommonResponse must be added into the new CommonResponse object</param>
        /// <typeparam name="TNewGenericType">The new generic type</typeparam>
        /// <typeparam name="TOldGenericType">The old generic Type</typeparam>
        /// <returns>The <see cref="SimpleResult{TData}" /> complete with errors and model in the new generic type</returns>
        /// <exception cref="ArgumentNullException">When commonResponse is null</exception>
        // public static ISimpleResult<TNewGenericType> Transform<TNewGenericType, TOldGenericType>(this ISimpleResult<TOldGenericType> simpleResult, TNewGenericType newTypeValue, bool addErrors)
        // {
        //     Check.ThrowIfNull(simpleResult, nameof(ISimpleResult<TOldGenericType>), new VanthException("Result object cannot be null."));
        //
        //     IResultBuilder<TNewGenericType> builder = new ResultBuilder<TNewGenericType>().WithData(newTypeValue);
        //     if (simpleResult.HasErrors() && addErrors)
        //     {
        //         builder.WithErrors(simpleResult.Errors);
        //     }
        //
        //     return builder.Build();
        // }
        public static string ToJson(this IError error)
        {
            return JsonConvert.SerializeObject(error);
        }

        /// <summary>
        ///     Clear the details list.
        /// </summary>
        public static void ClearDetails(this IError error)
        {
            if (error.Details.Any())
                error.Details.Clear();
        }

        public static string ToJson(this ErrorDetail detail)
        {
            return JsonConvert.SerializeObject(detail);
        }


    }
}

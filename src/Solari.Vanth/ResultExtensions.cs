using System;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public static class ResultExtensions
    {
        /// <summary>
        ///     Creates a new <see cref="Result{TModel}" /> with a different generic type. And add the new generic type value.
        /// </summary>
        /// <param name="commonResponse">CommonResponse object to be cloned.</param>
        /// <param name="newTypeValue">The new generic type value. Value must not be null.</param>
        /// <param name="addErrors">Indicates if the errors present in the old CommonResponse must be added into the new CommonResponse object</param>
        /// <typeparam name="TNewGenericType">The new generic type</typeparam>
        /// <typeparam name="TOldGenericType">The old generic Type</typeparam>
        /// <returns>The <see cref="Result{TModel}" /> complete with errors and model in the new generic type</returns>
        /// <exception cref="ArgumentNullException">When commonResponse is null</exception>
        public static Result<TNewGenericType> Transform<TNewGenericType, TOldGenericType>(
            this Result<TOldGenericType> commonResponse, TNewGenericType newTypeValue, bool addErrors)
        {
            if (commonResponse is null)
                throw new VanthException("Common response cannot be null.", new ArgumentNullException(nameof(commonResponse)));

            IResultBuilder<TNewGenericType> builder = new ResultBuilder<TNewGenericType>().WithData(newTypeValue);
            if (commonResponse.HasErrors() && addErrors)
            {
                builder.WithErrors(commonResponse.Errors);
            }

            return builder.Build();
        }

        public static T Map<T, TData>(this Result<TData> response, Func<Result<TData>, T> mappingFunction)
        {
            if (mappingFunction is null)
                throw new VanthException("Cannot invoke a null mapping function", new ArgumentNullException(nameof(mappingFunction)));
            return mappingFunction(response);
        }
    }
}

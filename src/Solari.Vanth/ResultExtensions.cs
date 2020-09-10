using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public static class ResultExtensions
    {
         /// <summary>
        ///     Adds <see cref="Error" /> into the stack.
        /// </summary>
        /// <param name="errorResponse">Error to be added</param>
        /// <returns>
        ///     <see cref="Result{TResult}" />
        /// </returns>
        public static Result<TData> AddError<TData>(this Result<TData> result, Error errorResponse)
         {
             result.Errors.Add(errorResponse);
            return result;
        }

        /// <summary>
        ///     Adds a stack of <see cref="Error" /> into the current stack.
        ///     It does not clear the current error stack before adding.
        /// </summary>
        /// <param name="errorResponse">Error to be added</param>
        /// <returns>
        ///     <see cref="Result{TResult}" />
        /// </returns>
        public static Result<TData> AddErrors<TData>(this Result<TData> result, List<Error> errorResponse)
        {
            if (errorResponse == null) throw new ArgumentNullException(nameof(errorResponse));
            foreach (Error commonErrorResponse in errorResponse) result.Errors.Add(commonErrorResponse);
            return result;
        }

        /// <summary>
        ///     Adds an <see cref="Error" /> into the stack.
        /// </summary>
        /// <param name="builder"><see cref="IErrorBuilder" /> delegate</param>
        /// <returns>
        ///     <see cref="Result{TResult}" />
        /// </returns>
        public static Result<TData> AddError<TData>(this Result<TData> result, Func<IErrorBuilder, Error> builder)
        {
            result.Errors.Add(builder(new ErrorBuilder()));
            return result;
        }

        /// <summary>
        ///     Adds an result.
        /// </summary>
        /// <param name="result">The result</param>
        /// <returns>
        ///     <see cref="Result{TResult}" />
        /// </returns>
        public static Result<TData> AddResult<TData>(this Result<TData> result, TData data)
        {
            result.Data = data;
            return result;
        }

        /// <summary>
        ///     Clear the error stack. This method also clear the details o each error in the stack.
        /// </summary>
        public static void ClearErrors<TData>(this Result<TData> result)
        {
            if (!result.Errors.Any()) return;
            foreach (Error commonErrorResponse in result.Errors) commonErrorResponse.ClearDetails();
            result.Errors.Clear();
        }

        public static string ToString<TData>(this Result<TData> result)
        {
            return JsonConvert.SerializeObject(result);
        }

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

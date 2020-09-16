using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using Newtonsoft.Json;
using Solari.Sol.Utils;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public static class VanthExtensions
    {
        public static bool HasStatusCode<TData>(this IResult<TData> result)
        {
            return result.StatusCode >= 200 && result.StatusCode < 300;
        }
        public static IResult<TData> AddStatusCode<TData>(this IResult<TData> result, int statusCode)
        {
            result.StatusCode = statusCode;
            return result;
        }

        /// <summary>
        ///     Adds <see cref="Error" /> into the stack.
        /// </summary>
        /// <param name="error">Error to be added</param>
        /// <returns>
        ///     <see cref="Result{TResult}" />
        /// </returns>
        public static IResult<TData> AddError<TData>(this IResult<TData> result, IError error)
        {
            result.Errors.Add(error);
            return result;
        }

        /// <summary>
        ///     Adds a stack of <see cref="Error" /> into the current stack.
        ///     It does not clear the current error stack before adding.
        /// </summary>
        /// <param name="errors">Error to be added</param>
        /// <returns>
        ///     <see cref="Result{TResult}" />
        /// </returns>
        public static IResult<TData> AddErrors<TData>(this IResult<TData> result, List<IError> errors)
        {
            if (errors == null) throw new ArgumentNullException(nameof(errors));
            result.Errors.AddRange(errors);
            return result;
        }

        /// <summary>
        ///     Adds an <see cref="Error" /> into the stack.
        /// </summary>
        /// <param name="builder"><see cref="IErrorBuilder" /> delegate</param>
        /// <returns>
        ///     <see cref="Result{TResult}" />
        /// </returns>
        public static IResult<TData> AddError<TData>(this IResult<TData> result, Func<IErrorBuilder, IError> builder)
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
        public static IResult<TData> AddResult<TData>(this IResult<TData> result, TData data)
        {
            result.Data = data;
            return result;
        }

        /// <summary>
        ///     Clear the error stack. This method also clear the details o each error in the stack.
        /// </summary>
        public static void ClearErrors<TData>(this IResult<TData> result)
        {
            if (!result.Errors.Any()) return;
            foreach (Error error in result.Errors) error.ClearDetails();
            result.Errors.Clear();
        }

        public static string ToString<TData>(this Result<TData> result)
        {
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        ///     Creates a new <see cref="Result{TModel}" /> with a different generic type. And add the new generic type value.
        /// </summary>
        /// <param name="result">CommonResponse object to be cloned.</param>
        /// <param name="newTypeValue">The new generic type value. Value must not be null.</param>
        /// <param name="addErrors">Indicates if the errors present in the old CommonResponse must be added into the new CommonResponse object</param>
        /// <typeparam name="TNewGenericType">The new generic type</typeparam>
        /// <typeparam name="TOldGenericType">The old generic Type</typeparam>
        /// <returns>The <see cref="Result{TModel}" /> complete with errors and model in the new generic type</returns>
        /// <exception cref="ArgumentNullException">When commonResponse is null</exception>
        public static IResult<TNewGenericType> Transform<TNewGenericType, TOldGenericType>(this IResult<TOldGenericType> result, TNewGenericType newTypeValue, bool addErrors)
        {
            if (result is null)
                throw new VanthException("Result object cannot be null.", new ArgumentNullException(nameof(result)));

            IResultBuilder<TNewGenericType> builder = new ResultBuilder<TNewGenericType>().WithData(newTypeValue);
            if (result.HasErrors() && addErrors)
            {
                builder.WithErrors(result.Errors);
            }

            return builder.Build();
        }

        public static T Map<T, TData>(this Result<TData> result, Func<Result<TData>, T> mappingFunction)
        {
            if (mappingFunction is null)
                throw new VanthException("Cannot invoke a null mapping function", new ArgumentNullException(nameof(mappingFunction)));
            return mappingFunction(result);
        }

        /// <summary>
        ///     Adds an <see cref="ErrorDetail" /> into the details list.
        /// </summary>
        /// <param name="detailedError">The detail of error</param>
        /// <returns>
        ///     <see cref="Error" />
        /// </returns>
        public static IError AddErrorDetail(this IError error, IErrorDetail detailedError)
        {
            error.Details.Add(detailedError);
            return error;
        }

        /// <summary>
        ///     Adds and <see cref="IEnumerable{T}" /> of <see cref="ErrorDetail" /> into the details list.
        ///     It does not clears the list.
        /// </summary>
        /// <param name="detailedErrors"></param>
        /// <returns>
        ///     <see cref="Error" />
        /// </returns>
        public static IError AddErrorDetail(this IError error, IEnumerable<IErrorDetail> detailedErrors)
        {
            error.Details.AddRange(detailedErrors);
            return error;
        }

        /// <summary>
        ///     Adds an <see cref="ErrorDetail" /> into the details list.
        /// </summary>
        /// <param name="detailedErrors"><see cref="IErrorDetailBuilder" /> delegate</param>
        /// <returns>
        ///     <see cref="Error" />
        /// </returns>
        public static IError AddErrorDetail(this IError error, Func<IErrorDetailBuilder, IErrorDetail> detailedErrors)
        {
            error.AddErrorDetail(detailedErrors(new ErrorDetailBuilder()));
            return error;
        }


        public static string ToString(this IError error)
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

        public static string ToString(this ErrorDetail detail)
        {
            return JsonConvert.SerializeObject(detail);
        }

        public static bool HasData<T>(this IResult<T> result)
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

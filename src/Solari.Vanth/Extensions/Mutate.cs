using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Newtonsoft.Json;
using Solari.Sol.Abstractions;
using Solari.Vanth.Builders;

namespace Solari.Vanth.Extensions
{
    public static partial class ResultExtensions
    {
        //TODO FIX
        // public static ISimpleResult AddError(this ISimpleResult simpleResult, IError error)
        // {
        //     Check.ThrowIfNull(error, nameof(IError));
        //     simpleResult.Errors.Add(error);
        //     return simpleResult;
        // }
        //
        // public static ISimpleResult AddError(this ISimpleResult simpleResult, List<IError> error)
        // {
        //     Check.ThrowIfNull(error, nameof(IError));
        //     simpleResult.Errors.AddRange(error);
        //     return simpleResult;
        // }
        // public static ISimpleResult AddStatusCode(this ISimpleResult simpleResult, int statusCode)
        // {
        //     Check.ThrowIfNull(statusCode, nameof(statusCode));
        //     simpleResult.StatusCode = statusCode;
        //     return simpleResult;
        // }
        //
        // public static ISimpleResult AddSuccess(this ISimpleResult simpleResult, bool success)
        // {
        //     Check.ThrowIfNull(success, nameof(success));
        //     simpleResult.Success = success;
        //     return simpleResult;
        // }
        // public static ISimpleResult<TData> AddSuccess<TData>(this ISimpleResult<TData> simpleResult, bool success)
        // {
        //     Check.ThrowIfNull(success, nameof(success));
        //     simpleResult.Success = success;
        //     return simpleResult;
        // }
        // public static ISimpleResult<TData> AddStatusCode<TData>(this ISimpleResult<TData> simpleResult, int statusCode)
        // {
        //     Check.ThrowIfNull(statusCode, nameof(statusCode));
        //     simpleResult.StatusCode = statusCode;
        //     return simpleResult;
        // }
        //
        // /// <summary>
        // ///     Adds <see cref="Error" /> into the stack.
        // /// </summary>
        // /// <param name="error">Error to be added</param>
        // /// <returns>
        // ///     <see cref="SimpleResult{TData}" />
        // /// </returns>
        // public static ISimpleResult<TData> AddError<TData>(this ISimpleResult<TData> simpleResult, IError error)
        // {
        //     Check.ThrowIfNull(error, nameof(IError));
        //     simpleResult.Errors.Add(error);
        //     return simpleResult;
        // }
        //
        // /// <summary>
        // ///     Adds a stack of <see cref="Error" /> into the current stack.
        // ///     It does not clear the current error stack before adding.
        // /// </summary>
        // /// <param name="errors">Error to be added</param>
        // /// <returns>
        // ///     <see cref="SimpleResult{TData}" />
        // /// </returns>
        // public static ISimpleResult<TData> AddErrors<TData>(this ISimpleResult<TData> simpleResult, List<IError> errors)
        // {
        //     Check.ThrowIfNull(errors, nameof(List<IError>));
        //     simpleResult.Errors.AddRange(errors);
        //     return simpleResult;
        // }
        //
        // /// <summary>
        // ///     Adds an <see cref="Error" /> into the stack.
        // /// </summary>
        // /// <param name="builder"><see cref="IErrorBuilder" /> delegate</param>
        // /// <returns>
        // ///     <see cref="SimpleResult{TData}" />
        // /// </returns>
        // public static ISimpleResult<TData> AddError<TData>(this ISimpleResult<TData> simpleResult, Func<IErrorBuilder, IError> builder)
        // {
        //     Check.ThrowIfNull(builder, nameof(Func<IErrorBuilder, IError>));
        //     simpleResult.Errors.Add(builder(new ErrorBuilder()));
        //     return simpleResult;
        // }
        //
        // /// <summary>
        // ///     Adds an result.
        // /// </summary>
        // /// <param name="simpleResult">The result</param>
        // /// <param name="data">The data to be </param>
        // /// <returns>
        // ///     <see cref="SimpleResult{TData}" />
        // /// </returns>
        // public static ISimpleResult<TData> AddData<TData>(this ISimpleResult<TData> simpleResult, TData data)
        // {
        //     Check.ThrowIfNull(data, nameof(TData));
        //     simpleResult.Data = data;
        //     return simpleResult;
        // }
        //
        // /// <summary>
        // ///     Clear the error stack. This method also clear the details o each error in the stack.
        // /// </summary>
        // public static void ClearErrors<TData>(this ISimpleResult<TData> simpleResult)
        // {
        //     if (!simpleResult.Errors.Any()) return;
        //     foreach (Error error in simpleResult.Errors) error.ClearDetails();
        //     simpleResult.Errors.Clear();
        // }
        //
        // public static string ToJson<TData>(this SimpleResult<TData> simpleResult)
        // {
        //     return JsonConvert.SerializeObject(simpleResult);
        // }
        //
        // /// <summary>
        // ///     Adds an <see cref="ErrorDetail" /> into the details list.
        // /// </summary>
        // /// <param name="detailedError">The detail of error</param>
        // /// <returns>
        // ///     <see cref="Error" />
        // /// </returns>
        // public static IError AddErrorDetail(this IError error, IErrorDetail detailedError)
        // {
        //     Check.ThrowIfNull(detailedError, nameof(IErrorDetail));
        //     error.Details.Add(detailedError);
        //     return error;
        // }
        //
        // /// <summary>
        // ///     Adds and <see cref="IEnumerable{T}" /> of <see cref="ErrorDetail" /> into the details list.
        // ///     It does not clears the list.
        // /// </summary>
        // /// <param name="detailedErrors"></param>
        // /// <returns>
        // ///     <see cref="Error" />
        // /// </returns>
        // public static IError AddErrorDetail(this IError error, IEnumerable<IErrorDetail> detailedErrors)
        // {
        //     Check.ThrowIfNull(detailedErrors, nameof(IEnumerable<IErrorDetail>));
        //     error.Details.AddRange(detailedErrors);
        //     return error;
        // }
        //
        // /// <summary>
        // ///     Adds an <see cref="ErrorDetail" /> into the details list.
        // /// </summary>
        // /// <param name="detailedErrors"><see cref="IErrorDetailBuilder" /> delegate</param>
        // /// <returns>
        // ///     <see cref="Error" />
        // /// </returns>
        // public static IError AddErrorDetail(this IError error, Func<IErrorDetailBuilder, IErrorDetail> detailedErrors)
        // {
        //     Check.ThrowIfNull(detailedErrors, nameof(Func<IErrorDetailBuilder, IErrorDetail>));
        //     error.AddErrorDetail(detailedErrors(new ErrorDetailBuilder()));
        //     return error;
        // }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Solari.Vanth.Builders;

namespace Solari.Vanth.Extensions
{
    public static class Result
    {
        // public static ISimpleResult Success()
        // {
        //     return new Vanth.SimpleResult()
        //     {
        //         Success = true,
        //         StatusCode = 200
        //     };
        // }
        //
        // public static ISimpleResult Error(List<IError> errors)
        // {
        //     return new Vanth.SimpleResult().AddError(errors).AddSuccess(false).AddStatusCode(CommonErrorCode.GenericErrorCodeInteger);
        // }
        // public static ISimpleResult<TData> Error<TData>(IError error)
        // {
        //
        // }
        //
        // public static ISimpleResult Error(IError error, int statusCode)
        // {
        //     return new Vanth.SimpleResult().AddError(error).AddSuccess(false).AddStatusCode(statusCode);
        // }
        //
        // public static ISimpleResult Error<TData>(Func<IErrorBuilder, IError> builder)
        // {
        //     return new Vanth.SimpleResult().AddError(builder(new ErrorBuilder())).AddSuccess(false).AddStatusCode()
        // }
        //
        // public static ISimpleResult<TData> Error<TData>(Func<IErrorBuilder, IError> builder, int statusCode)
        // {
        //
        // }
        // public static ISimpleResult<TData> Data<TData>(TData data)
        // {
        //     return Data(data, 200);
        // }
        //
        // public static ISimpleResult<TData> Data<TData>(TData data, int statusCode)
        // {
        //     return ResultBuilder<TData>.New.WithData(data).WithStatusCode(statusCode).Build();
        // }
        //
        // public static ISimpleResult<TData> Error<TData>(List<IError> errors)
        // {
        //     return new ResultBuilder<TData>().WithErrors(errors).WithStatusCode(CommonErrorCode.GenericErrorCodeInteger).Build();
        // }
        // public static ISimpleResult<TData> Error<TData>(IError error)
        // {
        //     return Error<TData>(error, CommonErrorCode.GenericErrorCodeInteger);
        // }
        //
        // public static ISimpleResult<TData> Error<TData>(IError error, int statusCode)
        // {
        //     return ResultBuilder<TData>.New.WithError(error).Build();
        // }
        //
        // public static ISimpleResult<TData> Error<TData>(Func<IErrorBuilder, IError> builder)
        // {
        //     return Error<TData>(builder, CommonErrorCode.GenericErrorCodeInteger);
        // }
        //
        // public static ISimpleResult<TData> Error<TData>(Func<IErrorBuilder, IError> builder, int statusCode)
        // {
        //     return ResultBuilder<TData>.New.WithError(builder).WithStatusCode(statusCode).Build();
        // }
        //
        // public static ISimpleResult<TData> ValidationError<TData>(ValidationResult result, string target = "")
        // {
        //     if (result == null) throw new VanthException("Cannot create a validation error without the validation result");
        //     if (result.IsValid || !result.Errors.Any())
        //         return new SimpleResult<TData>();
        //
        //
        //     IErrorBuilder error = DefaultError<TData>(CommonErrorCode.ValidationError,
        //                                               CommonErrorType.ValidationError,
        //                                               CommonErrorMessage.ValidationError,
        //                                               target);
        //
        //     foreach (ValidationFailure failure in result.Errors)
        //         error.WithDetail(builder => builder.WithErrorCode(failure.ErrorCode)
        //                                            .WithMessage(failure.ErrorMessage)
        //                                            .WithTarget($"Property: {failure.PropertyName}")
        //                                            .Build());
        //
        //     return DefaultResult<TData>(error, null, CommonErrorCode.ValidationError);
        // }
        //
        // public static ISimpleResult<TData> TransportError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string target = "")
        // {
        //     if (detailFunc == null)
        //         return TransportError<TData>(detail: null, target);
        //     return TransportError<TData>(detailFunc(new ErrorDetailBuilder()), target);
        // }
        //
        // public static ISimpleResult<TData> TransportError<TData>(IErrorDetail detail, string target = "")
        // {
        //     IErrorBuilder error = DefaultError<TData>(CommonErrorCode.TransportError,
        //                                               CommonErrorType.TransportError,
        //                                               CommonErrorMessage.TransportError,
        //                                               target);
        //     return DefaultResult<TData>(error, detail, CommonErrorCode.ValidationError);
        // }
        //
        // public static ISimpleResult<TData> NullObjectError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string operation, string target = "")
        // {
        //     if (detailFunc == null)
        //         return NullObjectError<TData>(detail: null, target);
        //     return NullObjectError<TData>(detailFunc(new ErrorDetailBuilder()), target);
        // }
        //
        // public static ISimpleResult<TData> NullObjectError<TData>(IErrorDetail detail, string operation, string target = "")
        // {
        //     IErrorBuilder error = DefaultError<TData>(CommonErrorCode.NullObjectError,
        //                                               CommonErrorType.NullObjectError,
        //                                               CommonErrorMessage.NullObjectError(operation),
        //                                               target);
        //     return DefaultResult<TData>(error, detail, CommonErrorCode.NullObjectError);
        // }
        //
        // public static ISimpleResult<TData> SerializationError<TData>(IErrorDetail detail, string target = "")
        // {
        //     IErrorBuilder error = DefaultError<TData>(CommonErrorCode.SerializationError,
        //                                               CommonErrorType.SerializationError,
        //                                               CommonErrorMessage.SerializationError,
        //                                               target);
        //     return DefaultResult<TData>(error, detail, CommonErrorCode.SerializationError);
        // }
        //
        // public static ISimpleResult<TData> SerializationError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string target = "")
        // {
        //     if (detailFunc == null)
        //         return SerializationError<TData>(detail: null, target);
        //     return SerializationError<TData>(detailFunc(new ErrorDetailBuilder()), target);
        // }
        //
        // public static ISimpleResult<TData> DatabaseError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string database, string target = "")
        // {
        //     if (detailFunc == null)
        //         return DatabaseError<TData>(detail: null, target);
        //     return DatabaseError<TData>(detailFunc(new ErrorDetailBuilder()), target);
        // }
        //
        // public static ISimpleResult<TData> DatabaseError<TData>(IErrorDetail detail, string database, string target = "")
        // {
        //     IErrorBuilder error = DefaultError<TData>(CommonErrorCode.DatabaseError,
        //                                               CommonErrorType.DatabaseError,
        //                                               CommonErrorMessage.DatabaseError(database),
        //                                               target);
        //     return DefaultResult<TData>(error, detail, CommonErrorCode.DatabaseError);
        // }
        //
        // public static ISimpleResult<TData> IntegrationError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string service, string target = "")
        // {
        //     if (detailFunc == null)
        //         return DatabaseError<TData>(detail: null, target);
        //     return DatabaseError<TData>(detailFunc(new ErrorDetailBuilder()), target);
        // }
        //
        // public static ISimpleResult<TData> IntegrationError<TData>(IErrorDetail detail, string service, string target = "")
        // {
        //     IErrorBuilder error = DefaultError<TData>(CommonErrorCode.IntegrationError,
        //                                               CommonErrorType.IntegrationError,
        //                                               CommonErrorMessage.IntegrationError(service),
        //                                               target);
        //     return DefaultResult<TData>(error, detail, CommonErrorCode.IntegrationError);
        // }
        //
        // public static ISimpleResult<TData> Exception<TData>(Exception exception, bool shouldAddStackTrace, string target = "")
        // {
        //     if (exception == null) throw new ArgumentNullException(nameof(exception), "Cannot create exception error from a null exception object");
        //
        //
        //     IErrorBuilder error = DefaultError<TData>(CommonErrorCode.ExceptionError,
        //                                               CommonErrorType.Exception,
        //                                               CommonErrorMessage.ExceptionError,
        //                                               target)
        //         .WithDetail(exception.ExtractDetailsFromException(shouldAddStackTrace));
        //     return DefaultResult<TData>(error, null, CommonErrorCode.ExceptionError);
        // }
        //
        // private static IErrorBuilder DefaultError<TData>(string errorCode, string errorType, string errorMessage, string target)
        // {
        //     IErrorBuilder error = new ErrorBuilder()
        //                           .WithCode(errorCode)
        //                           .WithMessage(errorMessage)
        //                           .WithErrorType(errorType)
        //                           .WithTarget(target);
        //     return error;
        // }
        //
        // private static ISimpleResult<TData> DefaultResult<TData>(IErrorBuilder builder, IErrorDetail detail, string errorCode)
        // {
        //     var statusCode = Convert.ToInt32(errorCode);
        //     if (detail is null)
        //         return new SimpleResult<TData>().AddError(builder.Build()).AddStatusCode(statusCode);
        //
        //     builder.WithDetail(detail);
        //     return new SimpleResult<TData>().AddError(builder.Build()).AddStatusCode(statusCode);
        // }
    }
}

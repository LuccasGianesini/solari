using System;
using FluentValidation.Results;
using Solari.Sol;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public interface IResultFactory
    {
        IResult<TData> Data<TData>(TData model);
        IResult<TData> Data<TData>(TData model, int statusCode);
        IResult<TData> Error<TData>(IError errorResponse);
        IResult<TData> Error<TData>(IError errorResponse, int statusCode);
        IResult<TData> Error<TData>(Func<IErrorBuilder, IError> builder);
        IResult<TData> Error<TData>(Func<IErrorBuilder, IError> builder, int statusCode);
        IResult<Nothing> Nothing();
        IResult<TData> Exception<TData>(Exception exception, bool shouldAddStackTrace, string target = "");
        IResult<TData> ValidationError<TData>(ValidationResult result, string target = "");
        IResult<TData> TransportError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string target = "");
        IResult<TData> TransportError<TData>(IErrorDetail detail, string target = "");
        IResult<TData> NullObjectError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string operation, string target = "");
        IResult<TData> NullObjectError<TData>(IErrorDetail detail, string operation, string target = "");
        IResult<TData> SerializationError<TData>(IErrorDetail detail, string target = "");
        IResult<TData> SerializationError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string target = "");
        IResult<TData> DatabaseError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string database, string target = "");
        IResult<TData> DatabaseError<TData>(IErrorDetail detail, string database, string target = "");
        IResult<TData> IntegrationError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string service, string target = "");
        IResult<TData> IntegrationError<TData>(IErrorDetail detail, string service, string target = "");
    }
}

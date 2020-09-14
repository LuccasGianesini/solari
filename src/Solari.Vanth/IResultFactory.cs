using System;
using FluentValidation.Results;
using Solari.Sol;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public interface IResultFactory
    {
        IResult<TData> FromData<TData>(TData model);
        IResult<TData> FromError<TData>(IError errorResponse);
        IResult<TData> FromError<TData>(Func<IErrorBuilder, IError> builder);
        IResult<Nothing> FromNothing();
        IResult<TData> ExceptionError<TData>(Exception exception, bool shouldAddStackTrace, string errorCode = "", string errorMessage = "");
        IResult<TData> FromError<TData>(ValidationResult result);
    }
}

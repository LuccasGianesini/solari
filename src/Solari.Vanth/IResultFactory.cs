using System;
using FluentValidation.Results;
using Solari.Sol;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public interface IResultFactory
    {
        Result<TData> FromData<TData>(TData model);
        Result<TData> FromError<TData>(Error errorResponse);
        Result<TData> FromError<TData>(Func<IErrorBuilder, Error> builder);
        Result<Nothing> FromNothing();
        Result<TData> ExceptionError<TData>(Exception exception, bool shouldAddStackTrace, string errorCode = "", string errorMessage = "");
        Result<TData> FromError<TData>(ValidationResult result);
    }
}

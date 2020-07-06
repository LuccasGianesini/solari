using System;
using FluentValidation.Results;
using Solari.Sol;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public interface IResultFactory
    {
        Result<TData> Success<TData>(TData model);
        Result<TData> Error<TData>(Error errorResponse);
        Result<TData> Error<TData>(Func<IErrorBuilder, Error> builder);
        Result<None> CreateEmpty();
        Result<TData> ExceptionError<TData>(Exception exception, string errorCode = "", string errorMessage = "");
        Result<TData> ValidationError<TData>(ValidationResult result);
    }
}

using System;
using System.Linq;
using FluentValidation.Results;
using Solari.Sol;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public class ResultFactory : IResultFactory
    {
        public Result<TData> Success<TData>(TData data) { return new ResultBuilder<TData>().WithResult(data).Build(); }

        public Result<TData> Error<TData>(Error error)
        {
            return new ResultBuilder<TData>().WithError(error).Build();
        }

        public Result<TData> Error<TData>(Func<IErrorBuilder, Error> builder)
        {
            return new ResultBuilder<TData>().WithError(builder).Build();
        }

        public Result<TData> ValidationError<TData>(ValidationResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (!result.Errors.Any())
                return Empty<TData>();


            Error error = new ErrorBuilder()
                                        .WithCode(CommonErrorCode.ValidationErrorCode)
                                        .WithErrorType(CommonErrorType.ValidationError)
                                        .WithMessage("Invalid Model State!")
                                        .Build();

            foreach (ValidationFailure failure in result.Errors)
                error.AddDetailedError(builder => builder.WithErrorCode(failure.ErrorCode)
                                                         .WithMessage(failure.ErrorMessage)
                                                         .WithTarget(failure.PropertyName)
                                                         .Build());

            return new Result<TData>().AddError(error);
        }

        public Result<None> CreateEmpty() { return new ResultBuilder<None>().WithResult(new None()).Build(); }

        public Result<TData> ExceptionError<TData>(Exception exception, string errorCode = "", string errorMessage = "")
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception), "Cannot create exception error from a null exception object");

            IErrorBuilder error = new ErrorBuilder()
                                                .WithCode(errorCode)
                                                .WithErrorType(CommonErrorType.Exception)
                                                .WithMessage(string.IsNullOrEmpty(errorMessage) ? exception.Message : errorMessage)
                                                .WithDetail(exception.ExtractDetailsFromException());
            return new Result<TData>().AddError(error.Build());
        }

        public Result<TData> Empty<TData>() { return new ResultBuilder<TData>().Build(); }
    }
}

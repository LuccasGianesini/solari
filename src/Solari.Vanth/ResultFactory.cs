using System;
using System.Linq;
using FluentValidation.Results;
using Solari.Sol;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public class ResultFactory : IResultFactory
    {
        public IResult<TData> FromData<TData>(TData data) { return new ResultBuilder<TData>().WithData(data).Build(); }

        public IResult<TData> FromError<TData>(IError error)
        {
            return new ResultBuilder<TData>().WithError(error).Build();
        }

        public IResult<TData> FromError<TData>(Func<IErrorBuilder, IError> builder)
        {
            return new ResultBuilder<TData>().WithError(builder).Build();
        }

        public IResult<TData> FromError<TData>(ValidationResult result)
        {
            if (result == null) throw new VanthException("Cannot create a validation error without the results.");
            if (!result.Errors.Any())
                return new Result<TData>();


            IError error = new ErrorBuilder()
                                        .WithCode(CommonErrorCode.ValidationErrorCode)
                                        .WithErrorType(CommonErrorType.ValidationError)
                                        .WithMessage("The provided object did not pass the validation.")
                                        .Build();

            foreach (ValidationFailure failure in result.Errors)
                error.AddErrorDetail(builder => builder.WithErrorCode(failure.ErrorCode)
                                                         .WithMessage(failure.ErrorMessage)
                                                         .WithTarget(failure.PropertyName)
                                                         .Build());

            return new Result<TData>().AddError(error);
        }

        public IResult<Nothing> FromNothing() { return new ResultBuilder<Nothing>().WithData(new Nothing()).Build(); }

        public IResult<TData> FromException<TData>(Exception exception,bool shouldAddStackTrace, string errorCode = "", string errorMessage = "")
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception), "Cannot create exception error from a null exception object");

            IErrorBuilder error = new ErrorBuilder()
                                                .WithCode(errorCode)
                                                .WithErrorType(CommonErrorType.Exception)
                                                .WithMessage(string.IsNullOrEmpty(errorMessage) ? exception.Message : errorMessage)
                                                .WithDetail(exception.ExtractDetailsFromException(shouldAddStackTrace));
            return new Result<TData>().AddError(error.Build());
        }
    }
}

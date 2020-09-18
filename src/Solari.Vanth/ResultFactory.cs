using System;
using System.Linq;
using FluentValidation.Results;
using Solari.Sol;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public class ResultFactory : IResultFactory
    {
        public IResult<TData> Data<TData>(TData data)
        {
            return Data(data, 200);
        }

        public IResult<TData> Data<TData>(TData data, int statusCode)
        {
            return new ResultBuilder<TData>().WithData(data).WithStatusCode(statusCode).Build();
        }

        public IResult<TData> Error<TData>(IError error)
        {
            return Error<TData>(error, Convert.ToInt32(CommonErrorCode.GenericErrorCode));
        }

        public IResult<TData> Error<TData>(IError error, int statusCode)
        {
            return new ResultBuilder<TData>().WithError(error).Build();
        }

        public IResult<TData> Error<TData>(Func<IErrorBuilder, IError> builder)
        {
            return new ResultBuilder<TData>().WithError(builder).Build();
        }

        public IResult<TData> ValidationError<TData>(ValidationResult result, string target = "")
        {
            if (result == null) throw new VanthException("Cannot create a validation error without the validation result");
            if (result.IsValid || !result.Errors.Any())
                return new Result<TData>();


            IErrorBuilder error = DefaultError<TData>(CommonErrorCode.ValidationError,
                                                      CommonErrorType.ValidationError,
                                                      CommonErrorMessage.ValidationError,
                                                      target);

            foreach (ValidationFailure failure in result.Errors)
                error.WithDetail(builder => builder.WithErrorCode(failure.ErrorCode)
                                                   .WithMessage(failure.ErrorMessage)
                                                   .WithTarget($"Property: {failure.PropertyName}")
                                                   .Build());

            return DefaultResult<TData>(error, null, CommonErrorCode.ValidationError);
        }

        public IResult<TData> TransportError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string target = "")
        {
            if (detailFunc == null)
                return TransportError<TData>(detail: null, target);
            return TransportError<TData>(detailFunc(new ErrorDetailBuilder()), target);
        }

        public IResult<TData> TransportError<TData>(IErrorDetail detail, string target = "")
        {
            IErrorBuilder error = DefaultError<TData>(CommonErrorCode.TransportError,
                                                      CommonErrorType.TransportError,
                                                      CommonErrorMessage.TransportError,
                                                      target);
            return DefaultResult<TData>(error, detail, CommonErrorCode.ValidationError);
        }

        public IResult<TData> NullObjectError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string operation, string target = "")
        {
            if (detailFunc == null)
                return NullObjectError<TData>(detail: null, target);
            return NullObjectError<TData>(detailFunc(new ErrorDetailBuilder()), target);
        }

        public IResult<TData> NullObjectError<TData>(IErrorDetail detail, string operation, string target = "")
        {
            IErrorBuilder error = DefaultError<TData>(CommonErrorCode.NullObjectError,
                                                      CommonErrorType.NullObjectError,
                                                      CommonErrorMessage.NullObjectError(operation),
                                                      target);
            return DefaultResult<TData>(error, detail, CommonErrorCode.NullObjectError);
        }

        public IResult<TData> SerializationError<TData>(IErrorDetail detail, string target = "")
        {
            IErrorBuilder error = DefaultError<TData>(CommonErrorCode.SerializationError,
                                                      CommonErrorType.SerializationError,
                                                      CommonErrorMessage.SerializationError,
                                                      target);
            return DefaultResult<TData>(error, detail, CommonErrorCode.SerializationError);
        }

        public IResult<TData> SerializationError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string target = "")
        {
            if (detailFunc == null)
                return SerializationError<TData>(detail: null, target);
            return SerializationError<TData>(detailFunc(new ErrorDetailBuilder()), target);
        }

        public IResult<TData> DatabaseError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string database, string target = "")
        {
            if (detailFunc == null)
                return DatabaseError<TData>(detail: null, target);
            return DatabaseError<TData>(detailFunc(new ErrorDetailBuilder()), target);
        }

        public IResult<TData> DatabaseError<TData>(IErrorDetail detail, string database, string target = "")
        {
            IErrorBuilder error = DefaultError<TData>(CommonErrorCode.DatabaseError,
                                                      CommonErrorType.DatabaseError,
                                                      CommonErrorMessage.DatabaseError(database),
                                                      target);
            return DefaultResult<TData>(error, detail, CommonErrorCode.DatabaseError);
        }

        public IResult<TData> IntegrationError<TData>(Func<IErrorDetailBuilder, IErrorDetail> detailFunc, string service, string target = "")
        {
            if (detailFunc == null)
                return DatabaseError<TData>(detail: null, target);
            return DatabaseError<TData>(detailFunc(new ErrorDetailBuilder()), target);
        }

        public IResult<TData> IntegrationError<TData>(IErrorDetail detail, string service, string target = "")
        {
            IErrorBuilder error = DefaultError<TData>(CommonErrorCode.IntegrationError,
                                                      CommonErrorType.IntegrationError,
                                                      CommonErrorMessage.IntegrationError(service),
                                                      target);
            return DefaultResult<TData>(error, detail, CommonErrorCode.IntegrationError);
        }

        public IResult<Nothing> Nothing()
        {
            return new ResultBuilder<Nothing>().WithData(new Nothing()).Build();
        }

        public IResult<TData> Exception<TData>(Exception exception, bool shouldAddStackTrace, string target = "")
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception), "Cannot create exception error from a null exception object");


            IErrorBuilder error = DefaultError<TData>(CommonErrorCode.ExceptionError,
                                                      CommonErrorType.Exception,
                                                      CommonErrorMessage.ExceptionError,
                                                      target)
                .WithDetail(exception.ExtractDetailsFromException(shouldAddStackTrace));
            return DefaultResult<TData>(error, null, CommonErrorCode.ExceptionError);
        }


        private IErrorBuilder DefaultError<TData>(string errorCode, string errorType, string errorMessage, string target)
        {
            IErrorBuilder error = new ErrorBuilder()
                                  .WithCode(errorCode)
                                  .WithMessage(errorMessage)
                                  .WithErrorType(errorType)
                                  .WithTarget(target);
            return error;
        }

        private IResult<TData> DefaultResult<TData>(IErrorBuilder builder, IErrorDetail detail, string errorCode)
        {
            var statusCode = Convert.ToInt32(errorCode);
            if (detail is null)
                return new Result<TData>().AddError(builder.Build()).AddStatusCode(statusCode);

            builder.WithDetail(detail);
            return new Result<TData>().AddError(builder.Build()).AddStatusCode(statusCode);
        }
    }
}

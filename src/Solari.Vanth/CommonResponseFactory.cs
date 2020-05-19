﻿using System;
using System.Linq;
using FluentValidation.Results;
using Solari.Sol;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public class CommonResponseFactory : ICommonResponseFactory
    {
        public CommonResponse<TResult> CreateResult<TResult>(TResult model) { return new CommonResponseBuilder<TResult>().WithResult(model).Build(); }

        public CommonResponse<TResult> CreateError<TResult>(CommonErrorResponse errorResponse)
        {
            return new CommonResponseBuilder<TResult>().WithError(errorResponse).Build();
        }

        public CommonResponse<TResult> CreateError<TResult>(Func<ICommonErrorResponseBuilder, CommonErrorResponse> builder)
        {
            return new CommonResponseBuilder<TResult>().WithError(builder).Build();
        }

        public CommonResponse<TResult> CreateError<TResult>(ValidationResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (!result.Errors.Any())
                return CreateEmpty<TResult>();


            CommonErrorResponse error = new CommonErrorResponseBuilder()
                                        .WithCode(CommonErrorCode.ValidationErrorCode)
                                        .WithErrorType(CommonErrorType.ValidationError)
                                        .WithMessage("Invalid Model State!")
                                        .Build();

            foreach (ValidationFailure failure in result.Errors)
                error.AddDetailedError(builder => builder.WithErrorCode(failure.ErrorCode)
                                                         .WithMessage(failure.ErrorMessage)
                                                         .WithTarget(failure.PropertyName)
                                                         .Build());

            return new CommonResponse<TResult>().AddError(error);
        }

        public CommonResponse<Empty> CreateEmpty() { return new CommonResponseBuilder<Empty>().WithResult(new Empty()).Build(); }

        public CommonResponse<TResult> CreateErrorFromException<TResult>(Exception exception, string errorCode = "", string errorMessage = "")
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception), "Cannot create exception error from a null exception object");

            ICommonErrorResponseBuilder error = new CommonErrorResponseBuilder()
                                                .WithCode(errorCode)
                                                .WithErrorType(CommonErrorType.Exception)
                                                .WithMessage(string.IsNullOrEmpty(errorMessage) ? exception.Message : errorMessage)
                                                .WithDetail(exception.ExtractDetailsFromException());
            return new CommonResponse<TResult>().AddError(error.Build());
        }

        public CommonResponse<TResult> CreateEmpty<TResult>() { return new CommonResponseBuilder<TResult>().Build(); }
    }
}
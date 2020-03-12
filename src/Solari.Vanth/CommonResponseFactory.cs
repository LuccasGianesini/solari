﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentValidation.Results;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Solari.Sol;
using Solari.Vanth.Builders;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Solari.Vanth
{
    public class CommonResponseFactory : ICommonResponseFactory
    {
        private readonly ApplicationOptions _application;
        public CommonResponseFactory(IOptions<ApplicationOptions> appOptions) { _application = appOptions.Value; }

        public CommonResponse<TResult> CreateResult<TResult>(TResult model) => new CommonResponseBuilder<TResult>().WithResult(model).Build();

        public CommonResponse<TResult> CreateError<TResult>(CommonErrorResponse errorResponse) =>
            new CommonResponseBuilder<TResult>().WithError(errorResponse).Build();

        public CommonResponse<TResult> CreateError<TResult>(Func<ICommonErrorResponseBuilder, CommonErrorResponse> builder) =>
            new CommonResponseBuilder<TResult>().WithError(builder).Build();

        public CommonResponse<TResult> CreateError<TResult>(ValidationResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (!result.Errors.Any())
                return CreateEmpty<TResult>();
            var response = new CommonResponse<TResult>();
            foreach (ValidationFailure failure in result.Errors)
            {
                response.AddError(builder => builder.WithCode(failure.ErrorCode)
                                                    .WithMessage(failure.ErrorMessage)
                                                    .WithTarget(failure.PropertyName)
                                                    .WithErrorType(CommonErrorType.ValidationError)
                                                    .WithInnerError(failure.ResourceName).Build());
            }

            return response;
        }

        public CommonResponse<Empty> CreateEmpty() => new CommonResponseBuilder<Empty>().WithResult(new Empty()).Build();
        public CommonResponse<TResult> CreateEmpty<TResult>() => new CommonResponseBuilder<TResult>().Build();

        public CommonResponse<TResult> CreateErrorFromException<TResult>(Exception exception, string errorCode = "", string errorMessage = "")
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception), "Cannot create exception error from a null exception object");
            
            ICommonErrorResponseBuilder error = new CommonErrorResponseBuilder()
                                                .WithCode(errorCode)
                                                .WithErrorType(CommonErrorType.Exception)
                                                .WithMessage(string.IsNullOrEmpty(errorMessage) ? exception.Message: errorMessage)
                                                .WithDetail(exception.ExtractDetailsFromException());
            return new CommonResponse<TResult>().AddError(error.Build());
        }
    }
}
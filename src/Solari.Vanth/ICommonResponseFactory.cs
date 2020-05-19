using System;
using FluentValidation.Results;
using Solari.Sol;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public interface ICommonResponseFactory
    {
        CommonResponse<TModel> CreateResult<TModel>(TModel model);
        CommonResponse<TModel> CreateError<TModel>(CommonErrorResponse errorResponse);
        CommonResponse<TModel> CreateError<TModel>(Func<ICommonErrorResponseBuilder, CommonErrorResponse> builder);
        CommonResponse<Empty> CreateEmpty();
        CommonResponse<TModel> CreateErrorFromException<TModel>(Exception exception, string errorCode = "", string errorMessage = "");
        CommonResponse<TResult> CreateError<TResult>(ValidationResult result);
    }
}
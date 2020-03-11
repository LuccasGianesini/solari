using System;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public interface ICommonResponseFactory
    {
        CommonResponse<TModel> CreateResult<TModel>(TModel model);
        CommonResponse<TModel> CreateError<TModel>(CommonErrorResponse errorResponse);
        CommonResponse<TModel> CreateError<TModel>(Func<ICommonErrorResponseBuilder, CommonErrorResponse> builder);
        CommonResponse<TModel> CreateEmpty<TModel>();
        
    }
}
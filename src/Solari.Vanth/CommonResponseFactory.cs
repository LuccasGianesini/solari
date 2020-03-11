using System;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public class CommonResponseFactory : ICommonResponseFactory
    {
        public CommonResponse<TModel> CreateResult<TModel>(TModel model) => new CommonResponseBuilder<TModel>().WithResult(model).Build();

        public CommonResponse<TModel> CreateError<TModel>(CommonErrorResponse errorResponse) =>
            new CommonResponseBuilder<TModel>().WithError(errorResponse).Build();

        public CommonResponse<TModel> CreateError<TModel>(Func<ICommonErrorResponseBuilder, CommonErrorResponse> builder) =>
            new CommonResponseBuilder<TModel>().WithError(builder).Build();

        public CommonResponse<TModel> CreateEmpty<TModel>() => new CommonResponseBuilder<TModel>().Build();
        
    }
}
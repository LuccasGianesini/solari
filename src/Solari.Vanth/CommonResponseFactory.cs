using System;
using Solari.Sol;
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

        public CommonResponse<Empty> CreateEmpty() => new CommonResponseBuilder<Empty>().WithResult(new Empty()).Build();

        public CommonResponse<TModel> CreateErrorFromException<TModel>(Exception exception, bool includeException = true, 
                                                                       string errorCode = "", string message = null, string detailMessage = null)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception), "Cannot create exception error from a null exception object");
            ICommonDetailedErrorResponseBuilder detailedErrorResponseBuilder = new CommonDetailedErrorResponseBuilder()
                                                                               .WithMessage(detailMessage ?? exception.Message)
                                                                               .WithSource(exception.Source)
                                                                               .WithTarget(exception.TargetSite.Name);
            if (includeException)
            {
                detailedErrorResponseBuilder.WithException(exception);
            }
            ICommonErrorResponseBuilder error = new CommonErrorResponseBuilder()
                                                .WithCode(errorCode)
                                                .WithMessage(message ?? exception.Message)
                                                .WithDetail(detailedErrorResponseBuilder.Build());
            return new CommonResponse<TModel>().AddError(error.Build());
        }
    }
}
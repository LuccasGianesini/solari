using System;
using System.Collections.Generic;

namespace Solari.Vanth.Builders
{
    public interface ICommonResponseBuilder<TResult>
    {
        ICommonResponseBuilder<TResult> WithResult(TResult model);
        ICommonResponseBuilder<TResult> WithError(CommonErrorResponse errorResponse);
        ICommonResponseBuilder<TResult> WithError(Func<ICommonErrorResponseBuilder, CommonErrorResponse> builder);
        ICommonResponseBuilder<TResult> WithErrors(Stack<CommonErrorResponse> errors);
        CommonResponse<TResult> Build();
    }
}
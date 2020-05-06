using System;
using System.Collections.Generic;
using System.Linq;

namespace Solari.Vanth.Builders
{
    public class CommonResponseBuilder<TResult> : ICommonResponseBuilder<TResult>
    {
        private readonly Stack<CommonErrorResponse> _errors = new Stack<CommonErrorResponse>(2);
        private TResult _model;

        public ICommonResponseBuilder<TResult> WithResult(TResult model)
        {
            _model = model;
            return this;
        }

        public ICommonResponseBuilder<TResult> WithError(CommonErrorResponse errorResponse)
        {
            _errors.Push(errorResponse);
            return this;
        }

        public ICommonResponseBuilder<TResult> WithError(Func<ICommonErrorResponseBuilder, CommonErrorResponse> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder), "Cannot invoke a null func");
            _errors.Push(builder(new CommonErrorResponseBuilder()));
            return this;
        }

        public ICommonResponseBuilder<TResult> WithErrors(Stack<CommonErrorResponse> errors)
        {
            foreach (CommonErrorResponse commonErrorResponse in errors) _errors.Push(commonErrorResponse);

            return this;
        }

        public CommonResponse<TResult> Build()
        {
            if (_model == null && _errors == null) return new CommonResponse<TResult>();
            return _errors.Any() ? new CommonResponse<TResult>().AddErrors(_errors) : new CommonResponse<TResult>().AddResult(_model);
        }
    }
}
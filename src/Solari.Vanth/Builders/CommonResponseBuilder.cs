using System;
using System.Collections.Generic;
using System.Linq;

namespace Solari.Vanth.Builders
{
    public class CommonResponseBuilder<TResult> : ICommonResponseBuilder<TResult>
    {
        private TResult _model;
        private readonly Stack<CommonErrorResponse> _errors = new Stack<CommonErrorResponse>(2);

        public ICommonResponseBuilder<TResult> WithResult(TResult model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
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
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            _errors.Push(builder(new CommonErrorResponseBuilder()));
            return this;
        }

        public ICommonResponseBuilder<TResult> WithErrors(Stack<CommonErrorResponse> errors)
        {
            if (errors.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(errors));

            foreach (CommonErrorResponse commonErrorResponse in errors)
            {
                _errors.Push(commonErrorResponse);
            }

            return this;
        }

        public CommonResponse<TResult> Build()
        {
            if (_model == null && _errors == null) return new CommonResponse<TResult>();
            return _errors.Any() ? new CommonResponse<TResult>().AddErrors(_errors) : new CommonResponse<TResult>().AddResult(_model);
        }
    }
}
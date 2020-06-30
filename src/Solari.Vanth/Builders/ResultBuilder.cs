using System;
using System.Collections.Generic;
using System.Linq;

namespace Solari.Vanth.Builders
{
    public class ResultBuilder<TResult> : IResultBuilder<TResult>
    {
        private readonly Stack<Error> _errors = new Stack<Error>(2);
        private TResult _model;

        public IResultBuilder<TResult> WithResult(TResult model)
        {
            _model = model;
            return this;
        }

        public IResultBuilder<TResult> WithError(Error errorResponse)
        {
            _errors.Push(errorResponse);
            return this;
        }

        public IResultBuilder<TResult> WithError(Func<IErrorBuilder, Error> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder), "Cannot invoke a null func");
            _errors.Push(builder(new ErrorBuilder()));
            return this;
        }

        public IResultBuilder<TResult> WithErrors(Stack<Error> errors)
        {
            foreach (Error commonErrorResponse in errors) _errors.Push(commonErrorResponse);

            return this;
        }

        public Result<TResult> Build()
        {
            if (_model == null && _errors == null) return new Result<TResult>();
            return _errors.Any() ? new Result<TResult>().AddErrors(_errors) : new Result<TResult>().AddResult(_model);
        }
    }
}

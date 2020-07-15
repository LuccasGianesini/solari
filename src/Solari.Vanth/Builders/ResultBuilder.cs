using System;
using System.Collections.Generic;
using System.Linq;

namespace Solari.Vanth.Builders
{
    public class ResultBuilder<TResult> : IResultBuilder<TResult>
    {
        private readonly List<Error> _errors = new List<Error>(2);
        private TResult _data;

        public IResultBuilder<TResult> WithData(TResult model)
        {
            _data = model;
            return this;
        }

        public IResultBuilder<TResult> WithError(Error errorResponse)
        {
            _errors.Add(errorResponse);
            return this;
        }

        public IResultBuilder<TResult> WithError(Func<IErrorBuilder, Error> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder), "Cannot invoke a null func");
            _errors.Add(builder(new ErrorBuilder()));
            return this;
        }

        public IResultBuilder<TResult> WithErrors(List<Error> errors)
        {
            foreach (Error commonErrorResponse in errors) _errors.Add(commonErrorResponse);

            return this;
        }

        public Result<TResult> Build()
        {
            if (_data == null && _errors == null) return new Result<TResult>();
            return new Result<TResult>().AddErrors(_errors).AddResult(_data);
        }
    }
}

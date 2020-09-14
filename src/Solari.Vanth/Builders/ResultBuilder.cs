using System;
using System.Collections.Generic;
using System.Linq;

namespace Solari.Vanth.Builders
{
    public class ResultBuilder<TData> : IResultBuilder<TData>
    {
        private readonly List<IError> _errors = new List<IError>(2);
        private TData _data;

        public IResultBuilder<TData> WithData(TData model)
        {
            _data = model;
            return this;
        }

        public IResultBuilder<TData> WithError(IError errorResponse)
        {
            _errors.Add(errorResponse);
            return this;
        }

        public IResultBuilder<TData> WithError(Func<IErrorBuilder, IError> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder), "Cannot invoke a null func");
            _errors.Add(builder(new ErrorBuilder()));
            return this;
        }

        public IResultBuilder<TData> WithErrors(List<IError> errors)
        {
            foreach (Error commonErrorResponse in errors) _errors.Add(commonErrorResponse);

            return this;
        }

        public IResult<TData> Build()
        {
            if (_data == null && _errors == null) return new Result<TData>();
            return new Result<TData>().AddErrors(_errors).AddResult(_data);
        }
    }
}

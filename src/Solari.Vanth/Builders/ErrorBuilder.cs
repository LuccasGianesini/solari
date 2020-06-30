using System;
using System.Collections.Generic;
using Solari.Vanth.Exceptions;

namespace Solari.Vanth.Builders
{
    public class ErrorBuilder : IErrorBuilder
    {
        private string _code;
        private readonly List<ErrorDetail> _details = new List<ErrorDetail>();
        private string _errorType;
        private object _innerError;
        private string _message;
        private string _target;


        public IErrorBuilder WithMessage(string message)
        {
            _message = message;
            return this;
        }

        public IErrorBuilder WithCode(string code)
        {
            _code = code;
            return this;
        }

        public IErrorBuilder WithErrorType(string type)
        {
            _errorType = type;
            return this;
        }

        public IErrorBuilder WithInnerError(object innerError)
        {
            _innerError = innerError;
            return this;
        }

        public IErrorBuilder WithTarget(string target)
        {
            _target = target;
            return this;
        }

        public IErrorBuilder WithDetail(ErrorDetail detail)
        {
            _details.Add(detail);
            return this;
        }

        public IErrorBuilder WithDetail(IEnumerable<ErrorDetail> details)
        {
            _details.AddRange(details);
            return this;
        }


        public IErrorBuilder WithDetail(Func<IErrorDetailBuilder, ErrorDetail> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder), "Cannot invoke a null func");
            _details.Add(builder(new ErrorDetailBuilder()));
            return this;
        }

        public Error Build()
        {
            if (string.IsNullOrEmpty(_message)) throw new NullOrEmptyErrorMessageException();
            return new Error(_code, _details, _errorType, _innerError, _message, _target);
        }
    }
}

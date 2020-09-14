using System;
using System.Collections.Generic;
using Solari.Vanth.Exceptions;

namespace Solari.Vanth.Builders
{
    public class ErrorBuilder : IErrorBuilder
    {
        private string _code;
        private readonly List<IErrorDetail> _details = new List<IErrorDetail>();
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

        public IErrorBuilder WithTarget(string target)
        {
            _target = target;
            return this;
        }

        public IErrorBuilder WithDetail(IErrorDetail detail)
        {
            _details.Add(detail);
            return this;
        }

        public IErrorBuilder WithDetail(IEnumerable<IErrorDetail> details)
        {
            _details.AddRange(details);
            return this;
        }


        public IErrorBuilder WithDetail(Func<IErrorDetailBuilder, IErrorDetail> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder), "Cannot invoke a null func");
            _details.Add(builder(new ErrorDetailBuilder()));
            return this;
        }

        public IError Build()
        {
            if (string.IsNullOrEmpty(_message)) throw new NullOrEmptyErrorMessageException();
            return new Error
            {
                Code = _code,
                ErrorType = _errorType,
                Message = _message,
                Target = _target
            }.AddErrorDetail(_details);
        }
    }
}

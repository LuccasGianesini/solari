using System;
using System.Text;
using Solari.Vanth.Exceptions;

namespace Solari.Vanth.Builders
{
    public class ErrorDetailBuilder : IErrorDetailBuilder
    {
        private string _code;
        private Exception _exception;
        private string _message;
        private string _source;
        private string _target;


        public IErrorDetailBuilder WithErrorCode(string code)
        {
            _code = code;
            return this;
        }

        public IErrorDetailBuilder WithException(Exception exception)
        {
            _exception = exception;
            return this;
        }

        public IErrorDetailBuilder WithMessage(string message)
        {
            _message = message;
            return this;
        }

        public IErrorDetailBuilder WithMessage(StringBuilder stringBuilder)
        {
            if (stringBuilder == null) throw new ArgumentNullException(nameof(stringBuilder));
            _message = stringBuilder.ToString();
            return this;
        }

        public IErrorDetailBuilder WithTarget(string target)
        {
            _target = target;
            return this;
        }

        public IErrorDetailBuilder WithSource(string source)
        {
            _source = source;
            return this;
        }

        public ErrorDetail Build()
        {
            if (string.IsNullOrEmpty(_message)) throw new NullOrEmptyErrorMessageException();
            return new ErrorDetail(_code, _message, _target, _source, _exception);
        }
    }
}

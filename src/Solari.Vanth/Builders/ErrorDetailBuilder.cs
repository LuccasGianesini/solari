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
        private string _helpUrl;
        private string _stackTrace;


        public IErrorDetailBuilder WithErrorCode(string code)
        {
            _code = code;
            return this;
        }
        public IErrorDetailBuilder WithHelpUrl(string helpUrl)
        {
            _helpUrl = helpUrl;
            return this;
        }
        public IErrorDetailBuilder WithStackTrace(string stackTrace)
        {
            _stackTrace = stackTrace;
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

        public IErrorDetail Build()
        {
            if (string.IsNullOrEmpty(_message)) throw new NullOrEmptyErrorMessageException();
            return new ErrorDetail
            {
                Code = _code,
                Message = _message,
                Target = _target,
                Source = _source,
                HelpUrl = _helpUrl,
                StackTrace = _stackTrace
            };
        }
    }
}

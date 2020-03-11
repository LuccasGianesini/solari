using System;
using System.Text;

namespace Solari.Vanth.Builders
{
    public class CommonDetailedErrorResponseBuilder : ICommonDetailedErrorResponseBuilder
    {
        private string _code;
        private Exception _exception;
        private string _message;
        private string _target;


        public ICommonDetailedErrorResponseBuilder WithErrorCode(string code)
        {
            if (string.IsNullOrEmpty(code)) throw new ArgumentException("Value cannot be null or empty.", nameof(code));
            _code = code;
            return this;
        }

        public ICommonDetailedErrorResponseBuilder WithException(Exception exception)
        {
            _exception = exception ?? throw new ArgumentNullException(nameof(exception));
            return this;
        }

        public ICommonDetailedErrorResponseBuilder WithMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Value cannot be null or empty.", nameof(message));
            _message = message;
            return this;
        }

        public ICommonDetailedErrorResponseBuilder WithMessage(StringBuilder stringBuilder)
        {
            if (stringBuilder == null) throw new ArgumentNullException(nameof(stringBuilder));
            _message = stringBuilder.ToString();
            return this;
        }

        public ICommonDetailedErrorResponseBuilder WithTarget(string target)
        {
            if (string.IsNullOrEmpty(target)) throw new ArgumentException("Value cannot be null or empty.", nameof(target));
            _target = target;
            return this;
        }

        public CommonDetailedErrorResponse Build()
        {
            if (_message == null) throw new ArgumentNullException(nameof(_message) ,"Error message cannot be null");
            return new CommonDetailedErrorResponse(_code, _message, _target, _exception);
        }
    }
}
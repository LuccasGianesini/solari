using System;
using System.Text;
using Solari.Vanth.Exceptions;

namespace Solari.Vanth.Builders
{
    public class CommonDetailedErrorResponseBuilder : ICommonDetailedErrorResponseBuilder
    {
        private string _code;
        private Exception _exception;
        private string _message;
        private string _target;
        private string _source;


        public ICommonDetailedErrorResponseBuilder WithErrorCode(string code)
        {
            _code = code;
            return this;
        }

        public ICommonDetailedErrorResponseBuilder WithException(Exception exception)
        {
            return this;
        }

        public ICommonDetailedErrorResponseBuilder WithMessage(string message)
        {
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
            _target = target;
            return this;
        }
        
        public ICommonDetailedErrorResponseBuilder WithSource(string source)
        {
            _source = source;
            return this;
        }

        public CommonDetailedErrorResponse Build()
        {
            if (string.IsNullOrEmpty(_message)) throw new NullOrEmptyErrorMessageException();
            return new CommonDetailedErrorResponse(_code, _message, _target, _source, _exception);
        }
    }
}
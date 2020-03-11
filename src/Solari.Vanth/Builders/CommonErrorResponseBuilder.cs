using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Solari.Vanth.Builders
{
    public class CommonErrorResponseBuilder : ICommonErrorResponseBuilder
    {
        private string _code;
        private List<CommonDetailedErrorResponse> _details;
        private string _errorType;
        private object _innerError;
        private string _message;
        private string _target;


        public ICommonErrorResponseBuilder WithMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Value cannot be null or empty.", nameof(message));
            _message = message;
            return this;
        }

        public ICommonErrorResponseBuilder WithCode(string code)
        {
            if (string.IsNullOrEmpty(code)) throw new ArgumentException("Value cannot be null or empty.", nameof(code));
            _code = code;
            return this;
        }

        public ICommonErrorResponseBuilder WithErrorType(string type)
        {
            if (string.IsNullOrEmpty(type)) throw new ArgumentException("Value cannot be null or empty.", nameof(type));
            _errorType = type;
            return this;
        }

        public ICommonErrorResponseBuilder WithInnerError(object innerError)
        {
            _innerError = innerError ?? throw new ArgumentNullException(nameof(innerError));
            return this;
        }

        public ICommonErrorResponseBuilder WithTarget(string target)
        {
            if (string.IsNullOrEmpty(target)) throw new ArgumentException("Value cannot be null or empty.", nameof(target));
            _target = target;
            return this;
        }

        public ICommonErrorResponseBuilder WithDetail(CommonDetailedErrorResponse detail)
        {
            if (_details == null) _details = new List<CommonDetailedErrorResponse>();
            _details.Add(detail);
            return this;
        }

        public ICommonErrorResponseBuilder WithDetail(IEnumerable<CommonDetailedErrorResponse> details)
        {
            if (_details == null) _details = new List<CommonDetailedErrorResponse>();
            _details.AddRange(details);
            return this;
        }


        public ICommonErrorResponseBuilder WithDetail(Func<ICommonDetailedErrorResponseBuilder, CommonDetailedErrorResponse> builder)
        {
            if (_details == null) _details = new List<CommonDetailedErrorResponse>();
            _details.Add(builder(new CommonDetailedErrorResponseBuilder()));
            return this;
        }

        public CommonErrorResponse Build()
        {
            if (_message == null) throw new ArgumentNullException(nameof(_message), "Error message cannot be null");
            return new CommonErrorResponse(_code, _details, _errorType, _innerError, _message, _target);
        }
    }
}
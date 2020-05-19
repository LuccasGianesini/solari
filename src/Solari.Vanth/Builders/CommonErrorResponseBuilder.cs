using System;
using System.Collections.Generic;
using Solari.Vanth.Exceptions;

namespace Solari.Vanth.Builders
{
    public class CommonErrorResponseBuilder : ICommonErrorResponseBuilder
    {
        private string _code;
        private readonly List<CommonDetailedErrorResponse> _details = new List<CommonDetailedErrorResponse>();
        private string _errorType;
        private object _innerError;
        private string _message;
        private string _target;


        public ICommonErrorResponseBuilder WithMessage(string message)
        {
            _message = message;
            return this;
        }

        public ICommonErrorResponseBuilder WithCode(string code)
        {
            _code = code;
            return this;
        }

        public ICommonErrorResponseBuilder WithErrorType(string type)
        {
            _errorType = type;
            return this;
        }

        public ICommonErrorResponseBuilder WithInnerError(object innerError)
        {
            _innerError = innerError;
            return this;
        }

        public ICommonErrorResponseBuilder WithTarget(string target)
        {
            _target = target;
            return this;
        }

        public ICommonErrorResponseBuilder WithDetail(CommonDetailedErrorResponse detail)
        {
            _details.Add(detail);
            return this;
        }

        public ICommonErrorResponseBuilder WithDetail(IEnumerable<CommonDetailedErrorResponse> details)
        {
            _details.AddRange(details);
            return this;
        }


        public ICommonErrorResponseBuilder WithDetail(Func<ICommonDetailedErrorResponseBuilder, CommonDetailedErrorResponse> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder), "Cannot invoke a null func");
            _details.Add(builder(new CommonDetailedErrorResponseBuilder()));
            return this;
        }

        public CommonErrorResponse Build()
        {
            if (string.IsNullOrEmpty(_message)) throw new NullOrEmptyErrorMessageException();
            return new CommonErrorResponse(_code, _details, _errorType, _innerError, _message, _target);
        }
    }
}
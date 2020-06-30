using System;
using System.Collections.Generic;
using Solari.Vanth.Exceptions;

namespace Solari.Vanth.Builders
{
    public interface IErrorBuilder
    {
        IErrorBuilder WithMessage(string message);
        IErrorBuilder WithCode(string code);
        IErrorBuilder WithErrorType(string type);
        IErrorBuilder WithInnerError(object innerError);
        IErrorBuilder WithTarget(string target);
        IErrorBuilder WithDetail(ErrorDetail detail);
        IErrorBuilder WithDetail(IEnumerable<ErrorDetail> details);
        IErrorBuilder WithDetail(Func<IErrorDetailBuilder, ErrorDetail> builder);

        /// <summary>
        ///     Build the response.
        /// </summary>
        /// <exception cref="NullOrEmptyErrorMessageException">When the error message is null or empty</exception>
        /// <returns>
        ///     <see cref="Error" />
        /// </returns>
        Error Build();
    }
}
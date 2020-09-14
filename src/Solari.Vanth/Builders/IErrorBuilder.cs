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
        IErrorBuilder WithTarget(string target);
        IErrorBuilder WithDetail(IErrorDetail detail);
        IErrorBuilder WithDetail(IEnumerable<IErrorDetail> details);
        IErrorBuilder WithDetail(Func<IErrorDetailBuilder, IErrorDetail> builder);

        /// <summary>
        ///     Build the response.
        /// </summary>
        /// <exception cref="NullOrEmptyErrorMessageException">When the error message is null or empty</exception>
        /// <returns>
        ///     <see cref="Error" />
        /// </returns>
        IError Build();
    }
}

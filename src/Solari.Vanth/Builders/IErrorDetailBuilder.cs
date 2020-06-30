using System;
using System.Text;
using Solari.Vanth.Exceptions;

namespace Solari.Vanth.Builders
{
    public interface IErrorDetailBuilder
    {
        IErrorDetailBuilder WithErrorCode(string code);
        IErrorDetailBuilder WithException(Exception exception);
        IErrorDetailBuilder WithMessage(string message);
        IErrorDetailBuilder WithMessage(StringBuilder stringBuilder);
        IErrorDetailBuilder WithTarget(string target);

        /// <summary>
        ///     Build the response.
        /// </summary>
        /// <exception cref="NullOrEmptyErrorMessageException">When the error message is null or empty</exception>
        /// <returns>
        ///     <see cref="Error" />
        /// </returns>
        ErrorDetail Build();

        IErrorDetailBuilder WithSource(string source);
    }
}
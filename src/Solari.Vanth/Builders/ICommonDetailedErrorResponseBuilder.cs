using System;
using System.Text;
using Solari.Vanth.Exceptions;

namespace Solari.Vanth.Builders
{
    public interface ICommonDetailedErrorResponseBuilder
    {
        ICommonDetailedErrorResponseBuilder WithErrorCode(string code);
        ICommonDetailedErrorResponseBuilder WithException(Exception exception);
        ICommonDetailedErrorResponseBuilder WithMessage(string message);
        ICommonDetailedErrorResponseBuilder WithMessage(StringBuilder stringBuilder);
        ICommonDetailedErrorResponseBuilder WithTarget(string target);

        /// <summary>
        ///     Build the response.
        /// </summary>
        /// <exception cref="NullOrEmptyErrorMessageException">When the error message is null or empty</exception>
        /// <returns>
        ///     <see cref="CommonErrorResponse" />
        /// </returns>
        CommonDetailedErrorResponse Build();

        ICommonDetailedErrorResponseBuilder WithSource(string source);
    }
}
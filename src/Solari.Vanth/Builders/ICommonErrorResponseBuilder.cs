using System;
using System.Collections.Generic;
using Solari.Vanth.Exceptions;

namespace Solari.Vanth.Builders
{
    public interface ICommonErrorResponseBuilder
    {
        ICommonErrorResponseBuilder WithMessage(string message);
        ICommonErrorResponseBuilder WithCode(string code);
        ICommonErrorResponseBuilder WithErrorType(string type);
        ICommonErrorResponseBuilder WithInnerError(object innerError);
        ICommonErrorResponseBuilder WithTarget(string target);
        ICommonErrorResponseBuilder WithDetail(CommonDetailedErrorResponse detail);
        ICommonErrorResponseBuilder WithDetail(IEnumerable<CommonDetailedErrorResponse> details);
        ICommonErrorResponseBuilder WithDetail(Func<ICommonDetailedErrorResponseBuilder, CommonDetailedErrorResponse> builder);

        /// <summary>
        ///     Build the response.
        /// </summary>
        /// <exception cref="NullOrEmptyErrorMessageException">When the error message is null or empty</exception>
        /// <returns>
        ///     <see cref="CommonErrorResponse" />
        /// </returns>
        CommonErrorResponse Build();
    }
}
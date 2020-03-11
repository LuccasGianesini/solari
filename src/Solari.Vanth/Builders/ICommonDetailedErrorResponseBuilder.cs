using System;
using System.Text;

namespace Solari.Vanth.Builders
{
    public interface ICommonDetailedErrorResponseBuilder
    {
        ICommonDetailedErrorResponseBuilder WithErrorCode(string code);
        ICommonDetailedErrorResponseBuilder WithException(Exception exception);
        ICommonDetailedErrorResponseBuilder WithMessage(string message);
        ICommonDetailedErrorResponseBuilder WithMessage(StringBuilder stringBuilder);
        ICommonDetailedErrorResponseBuilder WithTarget(string target);
        CommonDetailedErrorResponse Build();
    }
}
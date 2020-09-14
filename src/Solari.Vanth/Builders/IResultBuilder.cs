using System;
using System.Collections.Generic;

namespace Solari.Vanth.Builders
{
    public interface IResultBuilder<TData>
    {
        IResultBuilder<TData> WithData(TData model);
        IResultBuilder<TData> WithError(IError errorResponse);
        IResultBuilder<TData> WithError(Func<IErrorBuilder, IError> builder);
        IResultBuilder<TData> WithErrors(List<IError> errors);
        IResult<TData> Build();
    }
}

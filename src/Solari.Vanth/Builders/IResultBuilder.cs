using System;
using System.Collections.Generic;

namespace Solari.Vanth.Builders
{
    public interface IResultBuilder<TData>
    {
        IResultBuilder<TData> WithResult(TData model);
        IResultBuilder<TData> WithError(Error errorResponse);
        IResultBuilder<TData> WithError(Func<IErrorBuilder, Error> builder);
        IResultBuilder<TData> WithErrors(Stack<Error> errors);
        Result<TData> Build();
    }
}

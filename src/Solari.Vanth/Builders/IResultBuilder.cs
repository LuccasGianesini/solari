using System;
using System.Collections.Generic;

namespace Solari.Vanth.Builders
{
    public interface IResultBuilder<TData>
    {
        IResultBuilder<TData> WithData(TData model);
        IResultBuilder<TData> WithError(Error errorResponse);
        IResultBuilder<TData> WithError(Func<IErrorBuilder, Error> builder);
        IResultBuilder<TData> WithErrors(List<Error> errors);
        Result<TData> Build();
    }
}

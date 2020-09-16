using System;
using System.Collections.Generic;

namespace Solari.Vanth.Builders
{
    public interface IResultBuilder<TData>
    {
        IResultBuilder<TData> WithData(TData model);

        IResultBuilder<TData> WithStatusCode(int statusCode);
        IResultBuilder<TData> WithError(IError errorResponse);
        IResultBuilder<TData> WithError(Func<IErrorBuilder, IError> builder);
        IResultBuilder<TData> WithErrors(IEnumerable<IError> errors);
        IResult<TData> Build();
    }
}

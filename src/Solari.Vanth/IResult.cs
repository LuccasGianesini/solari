using System.Collections.Generic;

namespace Solari.Vanth
{
    public interface IResult<TData>
    {
        List<IError> Errors { get; set; }
        int StatusCode { get; set; }
        TData Data { get; set; }
    }
}

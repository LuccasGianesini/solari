using System.Collections.Generic;

namespace Solari.Vanth
{
    public interface IResult<TData>
    {
        List<IError> Errors { get; set; }
        TData Data { get; set; }
    }
}

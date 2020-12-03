using System.Collections.Generic;

namespace Solari.Vanth
{
    public interface ISimpleResult
    {
        List<IError> Errors { get; }
        bool Success { get; init; }
    }
}

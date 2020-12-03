using Solari.Sol.Abstractions;

namespace Solari.Vanth
{
    public interface IResult<T> : ISimpleResult
    {
        T Data { get; init; }
    }
}

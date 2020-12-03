using System.Collections.Generic;

namespace Solari.Vanth
{
    public record Result<T> : IResult<T>
    {
        public Result()
        {
            Errors = new List<IError>();
        }

        public T Data { get; init;  }
        public List<IError> Errors { get; }
        public bool Success { get; init; }
    }
}

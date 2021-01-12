using System.Collections.Generic;
using System.Runtime.Serialization;
using Solari.Sol.Abstractions;

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
        public IResult<T> AddError(IError error)
        {
            Check.ThrowIfNull(error, nameof(IError));
            Errors.Add(error);
            return this;
        }

        public IResult<T> AddError(List<IError> errors)
        {
            Check.ThrowIfNullOrEmpty(errors, nameof(List<IError>));
            Errors.AddRange(errors);
            return this;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Data), Data, typeof(T));
            info.AddValue(nameof(Errors), Errors, typeof(List<IError>));
            info.AddValue(nameof(Success), Success, typeof(bool));
        }
    }
}

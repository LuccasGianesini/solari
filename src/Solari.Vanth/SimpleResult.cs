using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Solari.Sol.Abstractions;
using Solari.Vanth.Builders;
using Solari.Vanth.Extensions;

namespace Solari.Vanth
{
    [Serializable]
    public record SimpleResult : ISimpleResult
    {
        public List<IError> Errors { get; init; }
        public bool Success { get; init; }
        public ISimpleResult AddError(IError error)
        {
            Check.ThrowIfNull(error, nameof(IError));
            Errors.Add(error);
            return this;
        }

        public ISimpleResult AddError(List<IError> errors)
        {
            Check.ThrowIfNullOrEmpty(errors, nameof(List<IError>));
            Errors.AddRange(errors);
            return this;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Errors), Errors, typeof(List<IError>));
            info.AddValue(nameof(Success), Success, typeof(bool));
        }
    }
}

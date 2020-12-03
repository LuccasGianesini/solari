using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Solari.Vanth.Builders;
using Solari.Vanth.Extensions;

namespace Solari.Vanth
{
    [Serializable]
    public class SimpleResult : ISimpleResult, ISerializable
    {
        public List<IError> Errors { get; set; }
        public bool Success { get; init; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Errors), Errors, typeof(List<Error>));
            info.AddValue(nameof(Success), Success, typeof(bool));
        }
    }
}

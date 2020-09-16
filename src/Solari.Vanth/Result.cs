using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Newtonsoft.Json;
using Solari.Sol.Utils;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    [Serializable]
    public class Result<TData> : IResult<TData>
    {
        public List<IError> Errors { get; set; } = new List<IError>();
        public int StatusCode { get; set; }
        public TData Data { get; set; }
    }
}

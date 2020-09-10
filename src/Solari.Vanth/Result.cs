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
    public class Result<TData>
    {
        public List<Error> Errors { get; set; } = new List<Error>();
        public TData Data { get; set; }
    }
}

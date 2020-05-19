using System.Collections.Generic;

namespace Solari.Callisto.Abstractions
{
    public interface ICallistoUpdate
    {
        List<string> Fields { get; set; }
    }
}
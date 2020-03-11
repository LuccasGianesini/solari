using System.Collections.Generic;

namespace Solari.Deimos.Abstractions
{
    public class HttpOptions
    {
        public List<string> IgnoredEndpoints { get; set; } = new List<string>();
        public bool UseMiddleware { get; set; }
    }
}
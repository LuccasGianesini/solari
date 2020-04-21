using System.Collections.Generic;

namespace Solari.Deimos.Abstractions
{
    public class HttpOptions
    {
        public List<string> IgnoredInEndpoints { get; set; } = new List<string>();
        public List<string> IgnoredOutEndpoints { get; set; } = new List<string>();
        public bool UseMiddleware { get; set; }
    }
}
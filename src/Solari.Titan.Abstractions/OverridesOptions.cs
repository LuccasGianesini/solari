using System.Collections.Generic;

namespace Solari.Titan.Abstractions
{
    public class OverridesOptions
    {
        public string Microsoft { get; set; } = "Warning";
        public string System { get; set; } = "Warning";
        public string MicrosoftHostingLifetime { get; set; } = "Warning";
        public List<string> Custom { get; set; } = new List<string>();
    }
}
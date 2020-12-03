using System.Collections.Generic;

namespace Solari.Io.Abstractions
{
    public class IoOptions
    {
        public bool Enabled { get; set; }
        public string HealthEndpoint { get; set; } = "/health";
    }
}

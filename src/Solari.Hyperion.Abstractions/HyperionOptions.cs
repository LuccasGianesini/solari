using System;
using Solari.Sol.Extensions;

namespace Solari.Hyperion.Abstractions
{
    public class HyperionOptions
    {
        public bool RegisterService { get; set; }
        public string ConsulAddress { get; set; }
        public string ConsulToken { get; set; }
        public string Datacenter { get; set; } = "dc1";
        public string WaitTime { get; set; }

        public HyperionHealthCheckOptions HealthCheck { get; set; } = new HyperionHealthCheckOptions();
        public HyperionConfigurationProviderOptions ConfigurationProvider { get; set; } = new HyperionConfigurationProviderOptions();

        public TimeSpan GetWaitTime() { return string.IsNullOrEmpty(WaitTime) ? TimeSpan.FromSeconds(30) : WaitTime.ToTimeSpan(); }
    }
}

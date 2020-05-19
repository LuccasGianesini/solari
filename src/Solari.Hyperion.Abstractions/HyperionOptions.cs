using System;
using Solari.Sol.Extensions;

namespace Solari.Hyperion.Abstractions
{
    public class HyperionOptions
    {
        public bool Register { get; set; } = true;
        public string ConsulAddress { get; set; }
        public string ConsulToken { get; set; }
        public string Datacenter { get; set; } = "dc1";

        public string WaitTime { get; set; }

        public HyperionHealthCheckOptions HealthCheck { get; set; } = new HyperionHealthCheckOptions();
        public HyperionConfigurationProviderOptions ConfigurationProvider { get; set; } = new HyperionConfigurationProviderOptions();

        public TimeSpan GetWaitTime() { return string.IsNullOrEmpty(WaitTime) ? TimeSpan.Zero : WaitTime.ToTimeSpan(); }
    }

    public class HyperionConfigurationProviderOptions
    {
        public string ConfigurationFileName { get; set; } = "appsettings";
        public string Path { get; set; }
    }

    public class HyperionHealthCheckOptions
    {
        public bool AddHealthCheck { get; set; }
        public string HealthEndpoint { get; set; } = "/health";
        public string CheckTimeout { get; set; } = "s5";
        public string CheckInterval { get; set; } = "s10";
        public bool TlsSkipVerify { get; set; } = true;
    }
}
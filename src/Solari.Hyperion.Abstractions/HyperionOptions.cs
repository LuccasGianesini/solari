namespace Solari.Hyperion.Abstractions
{
    public class HyperionOptions
    {
        public bool Register { get; set; } = true;
        public string ConsulAddress { get; set; }
        public string ConsulToken { get; set; }
        public string Datacenter { get; set; } = "dc1";
        public bool AddHealthCheck { get; set; }
        public string HealthEndpoint { get; set; } = "/health";
        public string CheckTimeout { get; set; } = "s5";
        public string CheckInterval { get; set; } = "s10";
        public bool TlsSkipVerify { get; set; } = true;
    }
}
namespace Solari.Hyperion.Abstractions
{
    public class HyperionHealthCheckOptions
    {
        public bool AddHealthCheck { get; set; }
        public string HealthEndpoint { get; set; } = "/health";
        public string CheckTimeout { get; set; } = "s5";
        public string CheckInterval { get; set; } = "s10";
        public bool TlsSkipVerify { get; set; } = true;
    }
}

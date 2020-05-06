namespace Solari.Ceres.Abstractions
{
    public class CeresOptions
    {
        public bool Enabled { get; set; }
        public bool UseTextEndpoint { get; set; } = true;
        public bool UseProtoEndpoint { get; set; } = true;
        public bool UseEnvEndpoint { get; set; } = true;
        public CpuUsageOptions Cpu { get; set; }
        public MemoryUsageOptions Memory { get; set; }
        public PrometheusOptions Prometheus { get; set; }
        public InfluxDbOptions InfluxDb { get; set; }

        public MetricsTrackingMiddlewareOptions Middlewares { get; set; }
    }
}
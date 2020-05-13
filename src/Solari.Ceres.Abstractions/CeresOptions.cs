namespace Solari.Ceres.Abstractions
{
    public class CeresOptions
    {
        public bool Enabled { get; set; }
        public bool CollectCpuMetrics { get; set; }
        public string CpuMetricsCollectorInterval { get; set; } = "s10";
        public bool CollectMemoryMetrics { get; set; }
        public string MemoryMetricsCollectorInterval { get; set; }
        public bool UseTextEndpoint { get; set; } = true;
        public bool UseProtoEndpoint { get; set; } = true;
        public bool UseEnvEndpoint { get; set; } = true;
        public PrometheusOptions Prometheus { get; set; }
        public InfluxDbOptions InfluxDb { get; set; }

        public MetricsTrackingMiddlewareOptions Middlewares { get; set; }
    }
}
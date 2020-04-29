using System.Collections.Generic;
using App.Metrics;
using App.Metrics.AspNetCore.Tracking.Internal;
using App.Metrics.Filtering;

namespace Solari.Ceres.Abstractions
{
    public class CeresOptions
    {
        public bool Enabled { get; set; } = true;
        public bool UseTextEndpoint { get; set; } = true;
        public bool UseProtoEndpoint { get; set; } = true;
        public bool UseEnvEndpoint { get; set; } = true;

        public PrometheusOptions Prometheus { get; set; } = new PrometheusOptions();
        public InfluxDbOptions InfluxDb { get; set; } = new InfluxDbOptions();

        public MetricsTrackingMiddlewareOptions Middlewares { get; set; } = new MetricsTrackingMiddlewareOptions();
    }
}
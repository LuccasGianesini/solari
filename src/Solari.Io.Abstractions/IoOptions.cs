using System.Collections.Generic;

namespace Solari.Io.Abstractions
{
    public class IoOptions
    {
        public bool Enabled { get; set; }
        public string HealthEndpoint { get; set; } = "/health";
        public bool EnableUi { get; set; }
        public int Interval { get; set; } = 20;
        public IoSeqPublisher Seq { get; set; }
        public IoPrometheusPublisher PrometheusGateway { get; set; }
        public List<IoHealthEndpoint> Endpoints { get; set; }
        public List<IoWebHookNotification> WebHooks { get; set; }
    }
}
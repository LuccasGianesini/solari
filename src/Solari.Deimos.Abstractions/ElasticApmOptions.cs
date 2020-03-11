using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Solari.Deimos.Abstractions
{
    public class ElasticApmOptions
    {
        public string Url { get; set; }
        public ElasticApmSubscribers Subscribers { get; set; } = new ElasticApmSubscribers();
        public string LogLevel { get; set; } = "Warning";
        public double TransactionSampleRate { get; set; } = 1.0;
        public int TransactionMaxSpans { get; set; } = 500;
        public string SecretToken { get; set; } 
        public bool VerifyServerCert { get; set; }
        public string FlushInterval { get; set; } = "10s";
        public int MaxBatchEventCount { get; set; } = 10;
        public int MaxQueueEventCount { get; set; } = 10;
        public string MetricsInterval { get; set; } = "30s";
        public List<string> DisableMetrics { get; set; } = new List<string>();
        public string CaptureBody { get; set; } = "off";
        public bool CaptureHeaders { get; set; }
        public string SpanFramesMinDuration { get; set; } = "5ms";
            


    }
}
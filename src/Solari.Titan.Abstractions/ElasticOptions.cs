using System;
using Serilog.Sinks.Elasticsearch;
using Solari.Io;

namespace Solari.Titan.Abstractions
{
    public class ElasticOptions
    {
        public bool AutoRegisterTemplate { get; set; } = true;
        public string AutoRegisterTemplateVersion { get; set; } = "";
        public string Url { get; set; }
        public bool BasicAuthEnabled { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string IndexFormat { get; set; }
        public int BufferFileCountLimit { get; set; } = 30;
        public string BufferLogShippingInterval { get; set; } = "s10";
        public string Period { get; set; } = "s2";
        public int QueueSizeLimit { get; set; } = 20;
        public int BatchPostingLimit { get; set; } = 100;
        public TimeSpan GetPeriod() => Period.ToTimeSpan();
        public TimeSpan GetBufferLogShippingInterval() => BufferLogShippingInterval.ToTimeSpan();
        public AutoRegisterTemplateVersion GetAutoRegisterTemplateVersion() =>
            AutoRegisterTemplateVersion.ToLowerInvariant() switch
            {
                "esv2" => Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv2,
                "esv5" => Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv5,
                "esv6" => Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv6,
                "esv7" => Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv7,
                _      => Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv7
            };
    }
}
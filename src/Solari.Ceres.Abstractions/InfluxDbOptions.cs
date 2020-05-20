using System;
using Solari.Sol.Extensions;

namespace Solari.Ceres.Abstractions
{
    public class InfluxDbOptions
    {
        public bool Enabled { get; set; }
        public string Uri { get; set; }
        public string Database { get; set; }
        public string Consistency { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool CreateDataBaseIfNotExists { get; set; } = true;
        public string RetentionPolicy { get; set; }
        public int FailuresBeforeBackoff { get; set; } = 3;
        public string Timeout { get; set; }
        public string BackoffPeriod { get; set; }

        public string Filter { get; set; }
        public string FlushInterval { get; set; }

        public TimeSpan GetFlushInterval() { return string.IsNullOrEmpty(FlushInterval) ? TimeSpan.FromSeconds(20) : FlushInterval.ToTimeSpan(); }


        public TimeSpan GetTimeout() { return string.IsNullOrEmpty(Timeout) ? TimeSpan.FromSeconds(20) : Timeout.ToTimeSpan(); }
        public TimeSpan GetBackoffPeriod() { return string.IsNullOrEmpty(BackoffPeriod) ? TimeSpan.FromSeconds(30) : BackoffPeriod.ToTimeSpan(); }
    }
}
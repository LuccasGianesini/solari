using Serilog.Events;
using Serilog.Sinks.Graylog.Core.Helpers;
using Serilog.Sinks.Graylog.Core.Transport;

namespace Solari.Titan.Abstractions
{
    public class GreyLogOptions
    {
        public bool Enabled { get; set; }
        public string LogLevelRestriction { get; set; } = "Warning";

        public string Address { get; set; }

        public int Port { get; set; }
        public string TransportType { get; set; }

        public int ShortMessageMaxLength { get; set; } = 500;

        public int MaxMessageSizeInUdp { get; set; } = 8192;

        public string Host { get; set; }

        public string Facility { get; set; } = "GELF";

        public int StackTraceDepth { get; set; } = 10;
        public string MessageGeneratorType { get; set; }

        public MessageIdGeneratorType GetMessageIdGeneratorType()
        {
            return MessageGeneratorType == null
                       ? MessageIdGeneratorType.Md5
                       : MessageGeneratorType.ToLowerInvariant().Equals("timestamp")
                           ? MessageIdGeneratorType.Timestamp
                           : MessageIdGeneratorType.Md5;
        }

        public LogEventLevel GetMinimumLogEventLevel()
        {
            if (LogLevelRestriction == null)
                return LogEventLevel.Warning;
            return LogLevelRestriction.ToLowerInvariant() switch
                   {
                       "verbose"     => LogEventLevel.Verbose,
                       "debug"       => LogEventLevel.Debug,
                       "information" => LogEventLevel.Information,
                       "warning"     => LogEventLevel.Warning,
                       "error"       => LogEventLevel.Error,
                       "fatal"       => LogEventLevel.Fatal,
                       _             => LogEventLevel.Warning
                   };
        }

        public TransportType GetTransportType()
        {
            if (TransportType == null)
                return Serilog.Sinks.Graylog.Core.Transport.TransportType.Udp;
            return TransportType.ToLowerInvariant() switch
                   {
                       "udp"  => Serilog.Sinks.Graylog.Core.Transport.TransportType.Udp,
                       "tcp"  => Serilog.Sinks.Graylog.Core.Transport.TransportType.Tcp,
                       "http" => Serilog.Sinks.Graylog.Core.Transport.TransportType.Http,
                       _      => Serilog.Sinks.Graylog.Core.Transport.TransportType.Udp
                   };
        }
    }
}
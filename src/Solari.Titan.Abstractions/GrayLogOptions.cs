using Serilog.Events;
using Serilog.Sinks.Graylog.Core.Helpers;
using Serilog.Sinks.Graylog.Core.Transport;

namespace Solari.Titan.Abstractions
{
    public class GrayLogOptions
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
                       : MessageGeneratorType.ToUpperInvariant().Equals("TIMESTAMP")
                           ? MessageIdGeneratorType.Timestamp
                           : MessageIdGeneratorType.Md5;
        }
        

        public TransportType GetTransportType()
        {
            if (TransportType == null)
                return Serilog.Sinks.Graylog.Core.Transport.TransportType.Udp;
            return TransportType.ToUpperInvariant() switch
                   {
                       "UDP"  => Serilog.Sinks.Graylog.Core.Transport.TransportType.Udp,
                       "TCP"  => Serilog.Sinks.Graylog.Core.Transport.TransportType.Tcp,
                       "HTTP" => Serilog.Sinks.Graylog.Core.Transport.TransportType.Http,
                       _      => Serilog.Sinks.Graylog.Core.Transport.TransportType.Udp
                   };
        }
    }
}
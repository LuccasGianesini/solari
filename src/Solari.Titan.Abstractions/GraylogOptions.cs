using Serilog.Events;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Helpers;
using Serilog.Sinks.Graylog.Core.Transport;

namespace Solari.Titan.Abstractions
{
    public class GraylogOptions
    {
        public bool Enabled { get; set; }
        public string MinimumLogEventLevel { get; set; }

        public string HostnameOrAddress { get; set; }

        public int Port { get; set; } = 12201;
        public string TransportType { get; set; }

        public int ShortMessageMaxLength { get; set; } = 1024;

        public int MaxMessageSizeInUdp { get; set; } = 8192;

        public string Host { get; set; }

        public string Facility { get; set; } = "GELF";

        public int StackTraceDepth { get; set; } = 5;
        public string MessageGeneratorType { get; set; }

        public MessageIdGeneratorType GetMessageIdGeneratorType()
            => MessageGeneratorType == null
                   ? MessageIdGeneratorType.Md5
                   : MessageGeneratorType.ToLowerInvariant().Equals("timestamp")
                       ? MessageIdGeneratorType.Timestamp
                       : MessageIdGeneratorType.Md5;


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

        public GraylogSinkOptions ToGraylogSinkOptions(bool dynamicLevel)
        {
            if (dynamicLevel)
                return BuildDefaultObject();
            else
            {
                GraylogSinkOptions opt = BuildDefaultObject();
                opt.MinimumLogEventLevel = TitanLibHelper.GetLogLevel(this.MinimumLogEventLevel);
                return opt;
            }

        }

        private GraylogSinkOptions BuildDefaultObject()
            => new GraylogSinkOptions
        {
            Host = this.Host,
            Facility = this.Facility,
            Port = this.Port,
            TransportType = this.GetTransportType(),
            HostnameOrAddress = this.HostnameOrAddress,
            StackTraceDepth = this.StackTraceDepth,
            MaxMessageSizeInUdp = this.MaxMessageSizeInUdp,
            ShortMessageMaxLength = this.ShortMessageMaxLength,
            MessageGeneratorType = this.GetMessageIdGeneratorType()
        };


    }
}

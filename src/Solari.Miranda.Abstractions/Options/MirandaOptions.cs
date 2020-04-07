using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using RawRabbit.Common;
using RawRabbit.Configuration;
using Solari.Io;

namespace Solari.Miranda.Abstractions.Options
{
    public class MirandaOptions
    {
        public bool EnableTitan { get; set; }
        public bool AutoCloseConnection { get; set; }
        public bool AutomaticRecovery { get; set; } = true;
        public MirandaExchangeOptions Exchange { get; set; } = new MirandaExchangeOptions();
        public string GracefulShutdownPeriod { get; set; }
        public List<string> Hostnames { get; set; } = new List<string>(5);
        public string Namespace { get; set; }
        public string Password { get; set; }
        public bool PersistentDeliveryMode { get; set; } = true;
        public int Port { get; set; }
        public string PublishConfirmTimeout { get; set; }
        public MirandaQueueOptions Queue { get; set; } = new MirandaQueueOptions();
        public string RequestTimeout { get; set; }
        public int Retries { get; set; }
        public string RetryInterval { get; set; }
        public bool RouteWithGlobalId { get; set; }
        public MirandaSslOptions Ssl { get; set; } = new MirandaSslOptions();
        public bool TopologyRecovery { get; set; } = true;
        public string Username { get; set; }
        public string VirtualHost { get; set; }

        public MirandaPluginsOptions Plugins { get; set; } = new MirandaPluginsOptions();
        public MirandaPolicyOptions Policies { get; set; } = new MirandaPolicyOptions();
        public MirandaQosOptions Qos { get; set; } = new MirandaQosOptions();
        public MirandaMessageProcessorOptions MessageProcessor { get; set; } = new MirandaMessageProcessorOptions();

        public SslOption GetSslOption()
        {
            return Ssl == null ? null : new SslOption(Ssl.ServerName, Ssl.CertificatePath, Ssl.Enabled);
        }

        public GeneralExchangeConfiguration GetExchangeConfiguration()
        {
            return Exchange == null
                       ? new GeneralExchangeConfiguration()
                       : new GeneralExchangeConfiguration
                       {
                           Durable = Exchange.Durable,
                           Type = Exchange.GetExchangeType(),
                           AutoDelete = Exchange.AutoDelete
                       };
        }

        public TimeSpan GetGracefulShutdownPeriod()
        {
            return string.IsNullOrEmpty(GracefulShutdownPeriod) ? TimeSpan.FromSeconds(5) : GracefulShutdownPeriod.ToTimeSpan();
        }

        public TimeSpan GetPublishConfirmTimeout()
        {
            return string.IsNullOrEmpty(PublishConfirmTimeout) ? TimeSpan.FromSeconds(5) : PublishConfirmTimeout.ToTimeSpan();
        }

        public GeneralQueueConfiguration GetQueueConfiguration()
        {
            return Queue == null
                       ? new GeneralQueueConfiguration()
                       : new GeneralQueueConfiguration
                       {
                           Durable = Queue.Durable,
                           Exclusive = Queue.Exclusive,
                           AutoDelete = Queue.AutoDelete
                       };
        }

        public TimeSpan GetRequestTimeout() { return string.IsNullOrEmpty(RequestTimeout) ? TimeSpan.FromSeconds(5) : RequestTimeout.ToTimeSpan(); }

        public TimeSpan GetRetryInterval() { return string.IsNullOrEmpty(RetryInterval) ? TimeSpan.FromSeconds(5) : RetryInterval.ToTimeSpan(); }
    }
}
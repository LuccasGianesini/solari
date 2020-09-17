using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Http;
using Serilog.Sinks.Loki;
using Serilog.Sinks.Loki.Labels;

namespace Solari.Titan.Loki
{
    public static class LokiSinkExtensions
    {
        public static LoggerConfiguration TitanLoki(this LoggerSinkConfiguration sinkConfiguration, LokiCredentials credentials,
                                                    ILogLabelProvider logLabelProvider, IHttpClient httpClient, LogEventLevel logLevelRestriction,
                                                    int batchPostingLimit, TimeSpan period, int queueLimit, bool dynamicLevel)
        {
            BatchFormatter formatter = logLabelProvider != null
                                           ? new BatchFormatter(logLabelProvider.GetLabels())
                                           : new BatchFormatter();

            IHttpClient client = httpClient ?? new LokiHttpClient();
            if (client is LokiHttpClient c)
            {
                c.SetAuthCredentials(credentials);
            }

            if (dynamicLevel)
                return sinkConfiguration.Http(LokiRouteBuilder.BuildPostUri(credentials.Url), batchFormatter: formatter, httpClient: client,
                                              batchPostingLimit: batchPostingLimit, period: period,
                                              queueLimit: queueLimit);

            return sinkConfiguration.Http(LokiRouteBuilder.BuildPostUri(credentials.Url), batchFormatter: formatter, httpClient: client,
                                          restrictedToMinimumLevel: logLevelRestriction, batchPostingLimit: batchPostingLimit, period: period,
                                          queueLimit: queueLimit);
        }
    }
}

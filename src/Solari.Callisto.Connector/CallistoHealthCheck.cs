using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto.Connector
{
    public static class HealthCheckExtensions
    {
        public static IHealthChecksBuilder AddCallistoHealthCheck(this IHealthChecksBuilder builder)
        {
            builder.AddCheck<CallistoHealthCheck>(CallistoConstants.Health);
            return builder;
        }
    }

    public class CallistoHealthCheck : IHealthCheck
    {
        private readonly ICallistoConnection _connection;
        private const string Description = "MongoDb connection state";

        public CallistoHealthCheck(ICallistoConnection connection)
        {
            _connection = connection;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                CallistoConnectionCheck status = await _connection.IsConnected(cancellationToken);
                return !status.IsConnected
                           ? HealthCheckResult.Degraded(Description, data: BuildDataDictionary(status))
                           : HealthCheckResult.Healthy(Description, BuildDataDictionary(status));
            }
            catch (MongoConnectionException e)
            {
                return HealthCheckResult.Unhealthy(Description, e);
            }
        }

        private Dictionary<string, object> BuildDataDictionary(CallistoConnectionCheck status)
        {
            return new Dictionary<string, object>()
            {
                {"ConnectionState", status.ClusterState.ToString()},
                {"PingResult", status.PingResult}
            };
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto.Connector
{
    public class CallistoHealthCheck : IHealthCheck
    {
        private const string Description = "MongoDb connection state";


        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                //TODO IMPLEMENT NEW WAY OF CHECKING HEALTH
                return HealthCheckResult.Healthy();
            }
            catch (MongoConnectionException e)
            {
                return HealthCheckResult.Unhealthy(Description, e);
            }
        }

        private Dictionary<string, object> BuildDataDictionary(CallistoConnectionCheck status)
        {
            return new Dictionary<string, object>
            {
                {"ConnectionState", status.ClusterState.ToString()},
                {"PingResult", status.PingResult}
            };
        }
    }
}

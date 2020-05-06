using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Consul;
using Solari.Hyperion.Abstractions;

namespace Solari.Hyperion
{
    public class ServiceOperations : IServiceOperations
    {
        private readonly IConsulClient _client;

        public ServiceOperations(IConsulClient client) { _client = client; }

        public async Task<IList<HyperionService>> GetServiceAddresses(string serviceName, string tag, bool healthyOnly = true)
        {
            QueryResult<ServiceEntry[]> result = await _client.Health.Service(serviceName, tag);
            return BuildReturn(serviceName, healthyOnly, result);
        }

        public async Task<IList<HyperionService>> GetServiceAddresses(string serviceName, bool healthyOnly = true)
        {
            QueryResult<ServiceEntry[]> result = await _client.Health.Service(serviceName);
            return BuildReturn(serviceName, healthyOnly, result);
        }

        private static IList<HyperionService> BuildReturn(string serviceName, bool healthyOnly, QueryResult<ServiceEntry[]> result)
        {
            if (result?.Response == null)
            {
                var s = new HttpResponseMessage();
                return new List<HyperionService>();
            }

            if (result.StatusCode == HttpStatusCode.InternalServerError)
            {
                return new List<HyperionService>();
            }

            if (healthyOnly)
            {
                return result.Response.SelectMany(a => a.Checks, (entry, check) =>
                             {
                                 if (check.Status.Status.Equals("passing"))
                                 {
                                     return new HyperionService
                                     {
                                         ServiceId = entry.Service.ID,
                                         ServiceName = serviceName,
                                         ServiceAddress = entry.Service.Address,
                                         ServicePort = entry.Service.Port
                                     };
                                 }

                                 return null;
                             })
                             .Where(a => a != null)
                             .Select(a => a)
                             .ToList();
            }

            return result.Response.SelectMany(a => a.Checks, (entry, check) => new HyperionService
            {
                ServiceId = entry.Service.ID,
                ServiceName = serviceName,
                ServiceAddress = entry.Service.Address,
                ServicePort = entry.Service.Port
            }).ToList();
        }
    }
}
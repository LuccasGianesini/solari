using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solari.Hyperion.Abstractions
{
    public interface IServiceOperations
    {
        Task<IList<HyperionService>> GetServiceAddresses(string serviceName, string tag, bool healthyOnly = true);
        Task<IList<HyperionService>> GetServiceAddresses(string serviceName, bool healthyOnly = true);
    }
}
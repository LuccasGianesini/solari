using Consul;
using Solari.Hyperion.Abstractions;


namespace Solari.Hyperion
{
    public class HyperionClient : IHyperionClient
    {
        public IConsulClient ConsulClient { get; }
        public IKvOperations Kv { get; }
        public IServiceOperations Services { get; }


        public HyperionClient(IConsulClient client, IKvOperations kvOperations, IServiceOperations serviceOperations)
        {
            ConsulClient = client;
            Kv = kvOperations;
            Services = serviceOperations;
        }

      
    }
}
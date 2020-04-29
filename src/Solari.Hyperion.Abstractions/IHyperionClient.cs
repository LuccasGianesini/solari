using Consul;

namespace Solari.Hyperion.Abstractions
{
    public interface IHyperionClient
    {
        IConsulClient ConsulClient { get; }
        IKvOperations Kv { get; }
        
        IServiceOperations Services { get; }
    }
}
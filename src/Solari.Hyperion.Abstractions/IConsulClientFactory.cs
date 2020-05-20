using System;
using Consul;

namespace Solari.Hyperion.Abstractions
{
    public interface IConsulClientFactory
    {
        IConsulClient Create(HyperionOptions options);

        IConsulClient Create(Func<HyperionOptions, HyperionOptions> options);

        IConsulClient Create(string address, string datacenter, string token, TimeSpan waitTime);
    }
}
using System;
using Consul;

namespace Solari.Hyperion.Abstractions
{
    public class ConsulClientFactory : IConsulClientFactory
    {
        public IConsulClient Create(HyperionOptions options)
        {
            if (options == null)
                throw new HyperionException($"Cannot create consul client with a null options {nameof(HyperionOptions)} object.");
            return new ConsulClient(BuildConfiguration(options.ConsulAddress, options.Datacenter, options.ConsulToken, options.GetWaitTime()));
        }

        public IConsulClient Create(Func<HyperionOptions, HyperionOptions> options)
        {
            if (options == null)
                throw new HyperionException($"Cannot invoke a null {nameof(Func<HyperionOptions, HyperionOptions>)}");
            return Create(options(new HyperionOptions()));
        }

        public IConsulClient Create(string address, string datacenter, string token, TimeSpan waitTime)
        {
            return new ConsulClient(BuildConfiguration(address, datacenter, token, waitTime));
        }


        private static Action<ConsulClientConfiguration> BuildConfiguration(string address, string datacenter, string token, TimeSpan waitTime)
        {
            if (string.IsNullOrEmpty(address))

                throw new HyperionException("Cannot create a Uri with a null or empty address.");

            return configuration =>
            {
                configuration.Address = new Uri(address);
                configuration.Datacenter = datacenter;
                configuration.Token = token;
                configuration.WaitTime = waitTime;
            };
        }
    }
}
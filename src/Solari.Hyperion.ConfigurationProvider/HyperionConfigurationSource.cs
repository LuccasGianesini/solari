using Microsoft.Extensions.Configuration;
using Solari.Hyperion.Abstractions;
using Solari.Sol;

namespace Solari.Hyperion.ConfigurationProvider
{
    public class HyperionConfigurationSource : IConfigurationSource
    {
        public HyperionOptions Options { get; set; }
        public ApplicationOptions ApplicationOptions { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder) { return new HyperionProvider(this); }
    }
}
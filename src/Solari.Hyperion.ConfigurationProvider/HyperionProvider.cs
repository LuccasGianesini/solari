namespace Solari.Hyperion.ConfigurationProvider
{
    public class HyperionProvider : Microsoft.Extensions.Configuration.ConfigurationProvider
    {
        private readonly HyperionConfigurationSource _source;

        public HyperionProvider(HyperionConfigurationSource source) { _source = source; }
        public override void Load() { base.Load(); }
    }
}
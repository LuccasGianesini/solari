namespace Solari.Hyperion.Abstractions
{
    public class HyperionConfigurationProviderOptions
    {
        public bool Enabled { get; set; }
        public string ConfigurationFileName { get; set; } = "appsettings";
        public string Path { get; set; }
    }
}

namespace Solari.Juno.Abstractions
{
    public class JunoConfigurationProviderOptions
    {
        public  bool Enabled { get; set; }

            public string SecretsBasePath { get; set; } = "/kv/data";
    }
}

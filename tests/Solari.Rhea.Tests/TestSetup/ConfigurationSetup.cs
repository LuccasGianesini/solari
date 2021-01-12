using Microsoft.Extensions.Configuration;

namespace Solari.Rhea.Tests.TestSetup
{
    public static class ConfigurationSetup
    {
        public static IConfiguration BuildConfiguration(ConfigurationKeys key) => new ConfigurationBuilder()
                                                                                  .AddInMemoryCollection(ConfigurationCollection.Select(key))
                                                                                  .Build();

    }
}

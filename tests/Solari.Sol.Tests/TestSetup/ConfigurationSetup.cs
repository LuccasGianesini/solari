using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Solari.Sol.Tests.TestSetup
{

    internal static class ConfigurationSetup
    {

        public static IConfiguration BuildConfiguration(ConfigurationKeys keys) => new ConfigurationBuilder()
                                                                                   .AddInMemoryCollection(ConfigurationCollection.Select(keys))
                                                                                   .Build();
    }
}

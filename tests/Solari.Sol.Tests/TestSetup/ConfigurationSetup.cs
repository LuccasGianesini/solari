using Microsoft.Extensions.Configuration;

namespace Solari.Sol.Tests.TestSetup
{

    internal static class ConfigurationSetup
    {

        public static IConfiguration BuildConfiguration(string configFileName)
        {
            return new ConfigurationBuilder().AddJsonFile(configFileName).Build();
        }


    }
}

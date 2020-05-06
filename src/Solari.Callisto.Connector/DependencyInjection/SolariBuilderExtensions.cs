using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Solari.Callisto.Abstractions;
using Solari.Sol;

namespace Solari.Callisto.Connector.DependencyInjection
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddCallistoConnector(this ISolariBuilder solariBuilder)
        {
            solariBuilder.Services.Configure<CallistoConnectorOptions>(solariBuilder
                                                                       .AppConfiguration
                                                                       .GetSection(CallistoConstants.ConnectorAppSettingsSection));
            solariBuilder.Services.AddSingleton<ICallistoConnectionFactory, CallistoConnectionFactory>();
            solariBuilder.Services.AddSingleton<ICallistoConnection, CallistoConnection>();
            solariBuilder.AddBuildAction(
                                         new BuildAction("Callisto Connector")
                                         {
                                             Action = provider =>
                                             {
                                                 var factory = provider.GetService<ICallistoConnectionFactory>();
                                                 var appOptions = provider.GetService<IOptions<ApplicationOptions>>();
                                                 var callistoOptions = provider.GetService<IOptions<CallistoConnectorOptions>>();
                                                 ICallistoConnection connection = provider.GetService<ICallistoConnection>()
                                                                                          .AddClient(factory.Make(callistoOptions.Value,
                                                                                                                  appOptions.Value).GetClient())
                                                                                          .AddDataBase(callistoOptions.Value.Database);
                                                 connection.IsConnected().GetAwaiter().GetResult();
                                             }
                                         });

            return solariBuilder;
        }
    }
}
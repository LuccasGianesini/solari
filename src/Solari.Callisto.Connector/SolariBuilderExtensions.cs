using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Solari.Callisto.Abstractions;
using Solari.Sol;

namespace Solari.Callisto.Connector
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
            // solariBuilder.Services.AddSingleton<ICallistoConnection, CallistoConnection>(provider =>
            // {
            //     var factory = provider.GetService<ICallistoConnectionFactory>();
            //     var appOptions = provider.GetService<IOptions<ApplicationOptions>>();
            //     var callistoOptions = provider.GetService<IOptions<CallistoConnectorOptions>>();
            //     return factory.Make(callistoOptions.Value, appOptions.Value);
            // });
            solariBuilder.AddBuildAction(provider =>
            {
                var factory = provider.GetService<ICallistoConnectionFactory>();
                var appOptions = provider.GetService<IOptions<ApplicationOptions>>();
                var callistoOptions = provider.GetService<IOptions<CallistoConnectorOptions>>();
                var mde = factory.Make(callistoOptions.Value, appOptions.Value);
                var connection = provider.GetService<ICallistoConnection>();
                connection.AddClient(mde.GetClient());
                connection.AddDataBase(callistoOptions.Value.Database);
                connection.IsConnected().GetAwaiter().GetResult();
            });

            return solariBuilder;
        }
    }
}
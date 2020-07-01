using Microsoft.Extensions.Configuration;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Sol.Extensions;

namespace Solari.Callisto.Integrations.MassTransit
{
    internal class Helper
    {
        public static CallistoConnectorOptions GetCallistoConnectionOptions(IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection(CallistoConstants.ConnectorAppSettingsSection);
            if (!section.Exists())
                throw new CallistoException("The callisto section does not exists in the AppSettings file.");
            var options = configuration.GetOptions<CallistoConnectorOptions>(section);
            if (string.IsNullOrEmpty(options.ConnectionString))
                throw new CallistoException("The current MassTransit integration requires a mongodb connection string");
            return options;
        }
    }
}

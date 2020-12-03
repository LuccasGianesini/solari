using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Sol;
using Solari.Sol.Abstractions;
using Solari.Sol.Abstractions.Extensions;

namespace Solari.Callisto.Integrations.MassTransit
{
    internal class Helper
    {
        public static CallistoConnectorOptions GetCallistoConnectionOptions(string clientName, IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection(CallistoConstants.ConnectorAppSettingsSection);
            if (!section.Exists())
                throw new CallistoException("The callisto section does not exists in the AppSettings file.");
            CallistoConnectorOptions options = section.GetOptions<List<CallistoConnectorOptions>>()
                                                      .FirstOrDefault(a => a.Name.ToUpperInvariant().Equals(clientName.ToUpperInvariant()));
            if (string.IsNullOrEmpty(options.ConnectionString))
                throw new CallistoException("The current MassTransit integration requires a mongodb connection string");
            return options;
        }

        public static ApplicationOptions GetAppOptions(IConfiguration configuration)
        {
            return configuration.GetOptions<ApplicationOptions>(SolariConstants.ApplicationAppSettingsSection);
        }
    }
}

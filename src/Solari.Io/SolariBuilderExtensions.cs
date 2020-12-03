using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Io.Abstractions;
using Solari.Sol;
using Solari.Sol.Abstractions;
using Solari.Sol.Abstractions.Extensions;

namespace Solari.Io
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddIo(this ISolariBuilder builder, Action<IHealthChecksBuilder> addChecks = null)
        {
            IConfigurationSection section = builder.Configuration.GetSection(IoConstants.AppSettingsSection);
            if (!section.Exists())
                throw new IOException("Io AppSettings section not found!");
            var options = section.GetOptions<IoOptions>();
            builder.Services.Configure<IoOptions>(section);
            if (!options.Enabled)
                return builder;
            IHealthChecksBuilder healthChecks = builder.Services.AddHealthChecks();
            addChecks?.Invoke(healthChecks);
            return builder;
        }
    }
}

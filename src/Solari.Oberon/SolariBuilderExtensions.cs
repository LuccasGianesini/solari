using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Sol;
using Solari.Sol.Extensions;

namespace Solari.Oberon
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddOberon(this ISolariBuilder builder)
        {
            IConfigurationSection section = builder.Configuration.GetSection(OberonLibConstants.AppSettingsSection);
            if (!section.Exists())
                throw new OberonException("Oberon AppSettings section does not exists.");

            var options = builder.Configuration.GetOptions<OberonOptions>(section);
            if (!options.Enabled)
                return builder;

            builder.Services.Configure<OberonOptions>(section);
            builder.Services.AddDistributedRedisCache(config =>
            {
                config.Configuration = options.ConnectionString;
                config.InstanceName = options.Instance;
            });

            builder.Services.AddSingleton<IOberon, Oberon>();
            return builder;
        }
    }
}

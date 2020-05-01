using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Sol;
using Solari.Sol.Extensions;
using StackExchange.Redis;

namespace Solari.Oberon
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddOberon(this ISolariBuilder builder)
        {
            IConfigurationSection section = builder.AppConfiguration.GetSection(OberonLibConstants.AppSettingsSection);
            if (!section.Exists())
                return builder;
            var options = builder.AppConfiguration.GetOptions<OberonOptions>(section);
            if (!options.Enabled)
                return builder;
            
            builder.Services.Configure<OberonOptions>(builder.AppConfiguration.GetSection(OberonLibConstants.AppSettingsSection));
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
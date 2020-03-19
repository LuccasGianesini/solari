using Microsoft.Extensions.DependencyInjection;
using Solari.Io;
using Solari.Sol;
using StackExchange.Redis;

namespace Solari.Oberon
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddOberon(this ISolariBuilder builder)
        {
            var options = builder.AppConfiguration.GetOptions<OberonOptions>(OberonLibConstants.AppSettingsSection);
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
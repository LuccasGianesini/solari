using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Hyperion.Abstractions;
using Solari.Sol;
using Solari.Sol.Abstractions;
using Solari.Sol.Abstractions.Extensions;

namespace Solari.Hyperion
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddHyperion(this ISolariBuilder builder)
        {
            HyperionOptions options = ConfigureHyperionOptions(builder.Services, builder.Configuration);
            AddHyperionCoreServices(builder.Services, options);
            RegisterApplication(builder.Services, options);
            return builder;
        }


        public static HyperionOptions ConfigureHyperionOptions(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            IConfigurationSection section = GetHyperionOptions(configuration, out HyperionOptions options);
            serviceCollection.Configure<HyperionOptions>(section);
            return options;
        }

        public static IConfigurationSection GetHyperionOptions(IConfiguration configuration, out HyperionOptions options)
        {
            IConfigurationSection section = configuration.GetSection(HyperionConstants.AppSettingsSection);
            if (!section.Exists())
                throw new HyperionException("Hyperion AppSettings section not found!");
            options = section.GetOptions<HyperionOptions>();
            return section;
        }


        public static void AddHyperionCoreServices(IServiceCollection serviceCollection, HyperionOptions options)
        {
            serviceCollection.AddSingleton<IHyperionClient, HyperionClient>();
            serviceCollection.AddSingleton<IKvOperations, KvOperations>();
            serviceCollection.AddSingleton<IServiceOperations, ServiceOperations>();
            serviceCollection.AddSingleton<IConsulClientFactory, ConsulClientFactory>();
            serviceCollection.AddSingleton(provider => provider.GetService<IConsulClientFactory>().Create(options));
        }

        public static void RegisterApplication(IServiceCollection serviceCollection, HyperionOptions hyperionOptions)
        {
            if (!hyperionOptions.RegisterService)
                return;
            serviceCollection.AddHostedService<HyperionStartupProcedure>();
        }
    }
}

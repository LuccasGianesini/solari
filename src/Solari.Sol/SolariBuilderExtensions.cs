using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Sol.Framework;

namespace Solari.Sol
{
    public static class SolariBuilderExtensions
    {
        
        /// <summary>
        /// Add sol into the DI container.
        /// <remarks>
        ///  Sol adds the following services into the DI:
        ///     Options;
        ///     Logging;
        ///     ApplicationOptions;
        ///     ISolariBuilder;
        ///     ISolariPostConfigure;
        ///     ISolariMarshal;
        /// </remarks>
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns></returns>
        public static ISolariBuilder AddSol(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddOptions();
            serviceCollection.AddLogging();
            serviceCollection.Configure<ApplicationOptions>(configuration.GetSection(SolariConstants.ApplicationAppSettingsSection));
            var instance = new SolariBuilder(serviceCollection, configuration);
            serviceCollection.AddSingleton<ISolariBuilder, SolariBuilder>(sp => instance);

            serviceCollection.AddSingleton<ISolariPostConfigure, SolariPostConfigure>();
            serviceCollection.AddSingleton<ISolariMarshal, SolariMarshal>();

            return instance;
        }
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Sol.Extensions;
using Solari.Sol.Framework;

namespace Solari.Sol
{
    public static class SolariBuilderExtensions
    {
        /// <summary>
        ///     Add sol into the DI container.
        ///     <remarks>
        ///         Sol adds the following services into the DI:
        ///         Options;
        ///         Logging;
        ///         ApplicationOptions;
        ///         ISolariBuilder;
        ///         ISolariPostConfigure;
        ///         ISolariMarshal;
        ///     </remarks>
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration">
        ///     <see cref="IConfiguration" />
        /// </param>
        /// <returns></returns>
        public static ISolariBuilder AddSol(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddOptions();
            serviceCollection.AddLogging();
            IConfigurationSection section = configuration.GetSection(SolariConstants.ApplicationAppSettingsSection);
            if (!section.Exists())
            {
                throw new SolException("'Application' AppSettings section not found");
            }

            var options = section.GetOptions<ApplicationOptions>();
            if (string.IsNullOrEmpty(options.ApplicationName))
            {
                throw new SolException("ApplicationName cannot be null or empty");
            }
            if (string.IsNullOrEmpty(options.ApplicationName))
            {
                throw new SolException("Project cannot be null or empty");
            }



            serviceCollection.Configure<ApplicationOptions>(configuration.GetSection(SolariConstants.ApplicationAppSettingsSection));
            var instance = new SolariBuilder(serviceCollection, configuration);
            serviceCollection.AddSingleton<ISolariBuilder, SolariBuilder>(sp => instance);

            serviceCollection.AddSingleton<ISolariPostConfigure, SolariPostConfigure>();
            serviceCollection.AddSingleton<ISolariMarshal, SolariMarshal>();

            return instance;
        }
    }
}

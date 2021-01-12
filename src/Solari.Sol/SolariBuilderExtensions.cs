using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Sol.Abstractions;
using Solari.Sol.Abstractions.Extensions;
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
            serviceCollection.AddOptions()
                             .AddLogging();

            IConfigurationSection section = configuration.GetSection(SolariConstants.ApplicationAppSettingsSection);

            Check.ThrowIfSectionDoesNotExists(section, new SolariException("'Application' AppSettings section not found"));

            var options = section.GetOptions<ApplicationOptions>();

            Check.ThrowIfNullOrEmpty(options.ApplicationName, new SolariException("ApplicationName cannot be null or empty"));
            Check.ThrowIfNullOrEmpty(options.Project, new SolariException("Project cannot be null or empty"));

            serviceCollection.Configure<ApplicationOptions>(configuration.GetSection(SolariConstants.ApplicationAppSettingsSection));
            var instance = new SolariBuilder(serviceCollection, configuration, options);

            serviceCollection.AddSingleton<ISolariBuilder, SolariBuilder>(sp => instance)
                             .AddSingleton<ISolariPostConfigure, SolariPostConfigure>()
                             .AddSingleton<ISolariMarshal, SolariMarshal>();

            return instance;
        }
    }
}

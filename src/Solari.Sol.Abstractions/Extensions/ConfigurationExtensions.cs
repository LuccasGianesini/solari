using Microsoft.Extensions.Configuration;
using Solari.Sol.Abstractions.Helpers;

namespace Solari.Sol.Abstractions.Extensions
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        ///     Gets an section of the json configuration and binds to an object.
        /// </summary>
        /// <param name="configuration">
        ///     <see cref="IConfiguration" />
        /// </param>
        /// <param name="section">Configuration section name</param>
        /// <typeparam name="TOptions">Resulting object</typeparam>
        /// <returns>Instance of TOptions</returns>
        public static TOptions GetOptions<TOptions>(this IConfiguration configuration, string section)
            where TOptions : class, new()
        {
            return configuration.GetSection(section).BindOptions<TOptions>();
        }

        /// <summary>
        ///     Gets an section of the json configuration and binds to an object.
        /// </summary>
        /// <param name="configuration">
        ///     <see cref="IConfiguration" />
        /// </param>
        /// <param name="section">Configuration section name</param>
        /// <typeparam name="TOptions">Resulting object</typeparam>
        /// <returns>Instance of TOptions</returns>
        public static TOptions GetOptions<TOptions>(this IConfigurationSection section)
            where TOptions : class, new()
        {
            return section.BindOptions<TOptions>();
        }


        public static ApplicationOptions GetApplicationOptions(this IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection(SolariConstants.ApplicationAppSettingsSection);
            if (!section.Exists())
            {
                throw new SolariException("The Application section was not found in the AppSettings file");
            }

            var options = section.GetOptions<ApplicationOptions>();
            if (string.IsNullOrEmpty(options.ApplicationName))
                throw new SolariException($"{nameof(ApplicationOptions.ApplicationName)} is a required value!");
            return options;
        }

        public static TOptions BindOptions<TOptions>(this IConfigurationSection section) where TOptions : class, new()
        {
            var options = new TOptions();
            section.Bind(options);
            return options;
        }
    }
}

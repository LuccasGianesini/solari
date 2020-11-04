using Microsoft.Extensions.Configuration;
using Solari.Sol.Helpers;

namespace Solari.Sol.Extensions
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
            return BinderHelper.BindOptions<TOptions>(configuration.GetSection(section));
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
            return BinderHelper.BindOptions<TOptions>(section);
        }


        public static ApplicationOptions GetApplicationOptions(this IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection(SolariConstants.ApplicationAppSettingsSection);
            if (!section.Exists())
            {
                throw new SolException("The Application section was not found in the AppSettings file");
            }

            var options = section.GetOptions<ApplicationOptions>();
            if (string.IsNullOrEmpty(options.ApplicationName))
                throw new SolException($"{nameof(ApplicationOptions.ApplicationName)} is a required value!");
            return options;
        }
    }
}

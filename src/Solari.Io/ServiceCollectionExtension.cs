using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Solari.Io
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        ///     Get a application from the service collection.
        ///     This method builds the <see cref="ServiceProvider" /> collection. Because of this behavior, the options must be registered in the
        ///     <see cref="IServiceCollection" /> prior to calling this method.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settingsSection"></param>
        /// <typeparam name="TOptions"></typeparam>
        /// <returns></returns>
        public static TOptions GetOptions<TOptions>(this IServiceCollection services, string settingsSection)
            where TOptions : class, new()
        {
            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            return BinderHelper.BindOptions<TOptions>(configuration.GetSection(settingsSection));
        }
    }
}
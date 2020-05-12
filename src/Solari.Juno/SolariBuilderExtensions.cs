using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Solari.Juno.Abstractions;
using Solari.Sol;
using Solari.Sol.Extensions;

namespace Solari.Juno
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddJuno(this ISolariBuilder builder)
        {
            builder.Services.AddJuno(builder.AppConfiguration);
            return builder;
        }

        public static IServiceCollection AddJuno(this IServiceCollection collection, IConfiguration configuration)
        {
            JunoOptions options = GetOptions(configuration);
            collection.AddSingleton(provider =>
            {
                var appOptions = provider.GetService<IOptions<ApplicationOptions>>();
                return BuildClient(options, appOptions.Value);
            });
            return collection;
        }


        public static IJunoClient BuildClient(JunoOptions options, ApplicationOptions applicationOptions)
        {
            return new JunoClientBuilder(applicationOptions)
                   .WithAddress(options.Address)
                   .WithAuthMethod(options.GetAuthMethodInfo())
                   .Build();
        }

        public static JunoOptions GetOptions(IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection(JunoConstants.AppSettingsSection);
            return !section.Exists() ? new JunoOptions() : configuration.GetOptions<JunoOptions>(section);
        }

        public static ApplicationOptions GetApplicationOptions(IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection(SolariConstants.ApplicationAppSettingsSection);
            return !section.Exists() ? new ApplicationOptions() : configuration.GetOptions<ApplicationOptions>(section);
        }
    }
}
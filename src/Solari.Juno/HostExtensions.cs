using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Solari.Juno.Abstractions;
using Solari.Sol;
using Solari.Sol.Extensions;
using Solari.Sol.Utils;

namespace Solari.Juno
{
    public static class HostExtensions
    {
        public static IWebHostBuilder UseJuno(this IWebHostBuilder webHostBuilder, bool addJunoServices)
        {
            webHostBuilder.ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                IConfigurationRoot conf = configurationBuilder.Build();
                ApplicationOptions app = conf.GetApplicationOptions();
                JunoOptions opt = SolariBuilderExtensions.GetOptions(conf);
                IDictionary<string, string> data = new JsonParser()
                    .Parse(JObject.FromObject(SolariBuilderExtensions.BuildClient(opt, app)
                                                                     .GetAppSettingsSecrets()
                                                                     .GetAwaiter()
                                                                     .GetResult()));
                configurationBuilder.Add(new MemoryConfigurationSource() {InitialData = data});
            });

            if (addJunoServices)
            {
                webHostBuilder.ConfigureServices((context, collection) => { collection.AddJuno(context.Configuration); });
            }

            return webHostBuilder;
        }

        public static IHostBuilder UseJuno(this IHostBuilder builder, bool addJunoServices)
        {
            builder.ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                IConfigurationRoot conf = configurationBuilder.Build();
                ApplicationOptions app = conf.GetApplicationOptions();
                JunoOptions opt = SolariBuilderExtensions.GetOptions(conf);
                IDictionary<string, string> data = new JsonParser()
                    .Parse(JObject.FromObject(SolariBuilderExtensions.BuildClient(opt, app)
                                                                     .GetAppSettingsSecrets()
                                                                     .GetAwaiter()
                                                                     .GetResult()));
                configurationBuilder.Add(new MemoryConfigurationSource() {InitialData = data});
            });

            if (addJunoServices)
            {
                builder.ConfigureServices((context, collection) => { collection.AddJuno(context.Configuration); });
            }

            return builder;
        }
    }
}

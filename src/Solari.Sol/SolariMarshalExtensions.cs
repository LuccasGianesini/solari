using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Solari.Sol
{
    public static class SolariMarshalExtensions
    {
        public static IApplicationBuilder UseSol(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.ApplicationServices.UseSol(applicationBuilder);
            return applicationBuilder;
        }

        public static IHost UseSol(this IHost host)
        {
            host.Services.UseSol(host: host);
            return host;
        }

        public static IServiceProvider UseSol(this IServiceProvider provider)
        {
            return provider.UseSol();
        }

        private static IServiceProvider UseSol(this IServiceProvider provider, IApplicationBuilder applicationBuilder = null
                                             , IHost host = null)
        {
            provider.GetRequiredService<ISolariMarshal>().ConfigureApplication(provider, applicationBuilder, host).ExecuteBuildActions();
            return provider;
        }
    }
}
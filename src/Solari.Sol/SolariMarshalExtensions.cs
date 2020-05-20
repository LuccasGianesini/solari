using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Solari.Sol
{
    public static class SolariMarshalExtensions
    {
        public static ISolariMarshal UseSol(this IServiceProvider provider, Func<IServiceProvider, ISolariMarshal> func) { return func(provider); }

        public static ISolariMarshal UseSol(this IApplicationBuilder builder, Func<IServiceProvider, ISolariMarshal> func)
        {
            return builder.ApplicationServices.UseSol(func);
        }

        public static ISolariMarshal UseSol(this IHost host, Func<IServiceProvider, ISolariMarshal> func) { return host.Services.UseSol(func); }

        public static ISolariMarshal UseSol(this IApplicationBuilder app) { return app.ApplicationServices.UseSol(app); }

        public static ISolariMarshal UseSol(this IHost host) { return host.Services.UseSol(host: host); }

        private static ISolariMarshal UseSol(this IServiceProvider provider, IApplicationBuilder applicationBuilder = null, IHost host = null)
        {
            return provider.UseSol(sp => sp.GetRequiredService<ISolariMarshal>()
                                           .ConfigureApplication(provider, applicationBuilder, host)
                                           .ExecuteBuildActions());
        }
    }
}
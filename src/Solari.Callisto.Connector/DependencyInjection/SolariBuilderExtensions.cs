using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTracing;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Tracer;
using Solari.Callisto.Tracer.Framework;
using Solari.Sol;
using Solari.Sol.Utils;

namespace Solari.Callisto.Connector.DependencyInjection
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddConnectorCoreServices(this ISolariBuilder builder)
        {
            builder.Services.AddSingleton(provider => CallistoClientRegistry.Instance);

            builder.Services.AddTransient<IEventFilter, EventFilter>();
            builder.Services.Configure<CallistoTracerOptions>(builder.Configuration.GetSection(CallistoConstants.TracerAppSettingsSection));
            builder.Services.AddSingleton<ICallistoEventListener>(provider =>
            {
                var eventFilter = provider.GetService<IEventFilter>();
                var callistoTracerOptions = provider.GetService<IOptions<CallistoTracerOptions>>();
                return new CallistoJaegerEventListener(provider.GetService<ITracer>(), eventFilter, callistoTracerOptions);
            });
            return builder;
        }

    }
}

using Microsoft.Diagnostics.Tracing.Compatibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTracing;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Tracer.Framework;
using Solari.Deimos.Abstractions;
using Solari.Io;
using Solari.Rhea;
using Solari.Sol;

namespace Solari.Callisto.Tracer
{
    public class CallistoTracerPlugin : ITracerPlugin
    {
        public void Configure()
        {
            Builder.Services.Configure<CallistoTracerOptions>(Builder.AppConfiguration.GetSection(CallistoConstants.TracerAppSettingsSection));
            Builder.Services.AddSingleton<ICallistoClientHook, CallistoClientHook>();
            Builder.Services.AddTransient<IEventFilter, EventFilter>();
            Builder.Services.AddSingleton<ICallistoEventListener>(provider =>
            {
                
                DeimosOptions options = provider.GetService<IOptions<DeimosOptions>>().Value;
                var eventFilter = provider.GetService<IEventFilter>();
                var callistoTracerOptions = provider.GetService<IOptions<CallistoTracerOptions>>();
                if (options.UseJaeger)
                {
                    return new CallistoJaegerEventListener(provider.GetService<ITracer>(), eventFilter, callistoTracerOptions);
                }

                if (!options.UseElasticApm) throw new ApplicationException("The application cannot be started because no tracer is available.");
                return new CallistoElasticEventListener(eventFilter, callistoTracerOptions);
            });
            Builder.AddBuildAction(new BuildAction("Deimos MongoDb Hook")
            {
                Action = provider =>
                {
                    var hook = provider.GetRequiredService<ICallistoClientHook>();
                    hook.AddHook();
                }
            });
        }

        public ISolariBuilder Builder { get; set; }
    }
}
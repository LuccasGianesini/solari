using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTracing;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Tracer.Framework;
using Solari.Deimos.Abstractions;
using Solari.Sol;
using Solari.Sol.Utils;

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
                var eventFilter = provider.GetService<IEventFilter>();
                var callistoTracerOptions = provider.GetService<IOptions<CallistoTracerOptions>>();
                return new CallistoJaegerEventListener(provider.GetService<ITracer>(), eventFilter, callistoTracerOptions);
            });
            Builder.AddBuildAction(new BuildAction("Deimos MongoDb Hook")
            {
                Action = provider =>
                {
                    try
                    {
                        var hook = provider.GetService<ICallistoClientHook>();
                        if(hook is null)
                            return;
                        hook.AddHook();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    
                }
            });
        }

        public ISolariBuilder Builder { get; set; }
    }
}
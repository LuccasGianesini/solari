using Jaeger;
using Jaeger.Reporters;
using Jaeger.Senders.Thrift;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTracing;
using OpenTracing.Contrib.NetCore.Configuration;
using OpenTracing.Contrib.NetCore.CoreFx;
using OpenTracing.Util;
using Solari.Deimos.Abstractions;
using Solari.Sol;
using Solari.Sol.Abstractions;
using Solari.Sol.Abstractions.Extensions;

namespace Solari.Deimos
{
    public static class JaegerTracerConfiguration
    {
        public static ISolariBuilder AddJaeger(ISolariBuilder solariBuilder, DeimosOptions options)
        {
            if (!options.Enabled)
                return solariBuilder;

            ConfigureHttpOut(solariBuilder, options);
            ConfigureHttpIn(solariBuilder, options);
            ConfigureTracer(solariBuilder, options.Jaeger);
            return solariBuilder;
        }

        private static void ConfigureHttpOut(ISolariBuilder builder, DeimosOptions options)
        {
            builder.Services.PostConfigure<HttpHandlerDiagnosticOptions>(conf =>
            {
                conf.InjectEnabled = message => true;
                foreach (string httpIgnoredEndpoint in options.Http.IgnoredOutEndpoints)
                    conf.IgnorePatterns.Add(context => context.RequestUri.OriginalString.Contains(httpIgnoredEndpoint));

                DeimosLogger.JaegerLogger.ConfiguredHttpOut();
            });
        }

        private static void ConfigureHttpIn(ISolariBuilder builder, DeimosOptions options)
        {
            builder.Services.AddOpenTracing(build => build.ConfigureAspNetCore(diagnosticOptions =>
            {
                ConfigureHttpInRequestFiltering(diagnosticOptions, options);

                diagnosticOptions.Hosting.ExtractEnabled = message => true;
                DeimosLogger.JaegerLogger.ConfiguredHttpIn();
            }));
        }

        private static void ConfigureHttpInRequestFiltering(AspNetCoreDiagnosticOptions diagnosticOptions, DeimosOptions options)
        {
            if (options == null) return;

            foreach (string httpIgnoredEndpoint in options.Http.IgnoredInEndpoints)
            {
                DeimosLogger.JaegerLogger.ConfigureRequestFiltering(httpIgnoredEndpoint);
                diagnosticOptions.Hosting.IgnorePatterns.Add(context => context.Request.Path
                                                                               .ToUriComponent()
                                                                               .Contains(PathString.FromUriComponent(httpIgnoredEndpoint)));
            }
        }


        private static ISolariBuilder ConfigureTracer(ISolariBuilder builder, JaegerOptions options)
        {
            builder.Services.AddSingleton(sp =>
            {
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                ApplicationOptions appOptions = sp.GetService<IOptions<ApplicationOptions>>().Value;
                ITracer tracer = BuildTracer(options, appOptions, loggerFactory);
                GlobalTracer.Register(tracer);
                DeimosLogger.JaegerLogger.ConfiguredTracer();
                DeimosLogger.JaegerLogger.UsingJaegerTracing();
                return tracer;
            });

            return builder;
        }

        private static ITracer BuildTracer(JaegerOptions options, ApplicationOptions appOptions, ILoggerFactory loggerFactory)
        {
            return new Tracer.Builder(appOptions.ApplicationName.DashToLower())
                   .WithLoggerFactory(loggerFactory)
                   .WithReporter(BuildRemoteReporter(options, loggerFactory))
                   .WithSampler(options.GetSampler())
                   .WithTag("app.instance.id", appOptions.ApplicationInstanceId)
                   .WithTag("app", appOptions.ApplicationName)
                   .Build();
        }

        private static RemoteReporter BuildRemoteReporter(JaegerOptions options, ILoggerFactory loggerFactory)
        {
            DeimosLogger.JaegerLogger.UdpRemoteReporter(options.UdpHost, options.UdpPort);
            return new RemoteReporter.Builder()
                   .WithSender(new UdpSender(options.UdpHost, options.UdpPort, options.MaxPacketSize))
                   .WithLoggerFactory(loggerFactory)
                   .Build();
        }
    }
}

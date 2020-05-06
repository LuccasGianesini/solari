using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
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

namespace Solari.Deimos
{
    public static class JaegerTracerConfiguration
    {
        public static ISolariBuilder AddJaeger(ISolariBuilder solariBuilder, DeimosOptions options)
        {
            ConfigureHttpOut(solariBuilder, options);
            ConfigureHttpIn(solariBuilder, options);
            ConfigureTracer(solariBuilder, options);
            return solariBuilder;
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


        private static ISolariBuilder ConfigureTracer(ISolariBuilder builder, DeimosOptions options)
        {
            if (!options.TracingEnabled) return builder;

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

        private static string BuildServiceName(ApplicationOptions applicationOptions, DeimosOptions deimosOptions)
        {
            string name = string.IsNullOrEmpty(deimosOptions.Jaeger.ServiceName)
                              ? applicationOptions.ApplicationName
                              : deimosOptions.Jaeger.ServiceName;

            return name.Contains("[env]")
                       ? name.Replace("[env]", applicationOptions.ApplicationEnvironment).ToLowerInvariant()
                       : (name + "." + applicationOptions.ApplicationEnvironment).ToLowerInvariant();
        }

        private static ITracer BuildTracer(DeimosOptions options, ApplicationOptions appOptions, ILoggerFactory loggerFactory)
        {
            return new Tracer.Builder(BuildServiceName(appOptions, options))
                   .WithLoggerFactory(loggerFactory)
                   .WithReporter(BuildRemoteReporter(options, appOptions, loggerFactory))
                   .WithSampler(GetSampler(options.Jaeger))
                   .WithTag("app.instance.id", appOptions.ApplicationInstanceId)
                   .WithTag("app.name", appOptions.ApplicationName)
                   .Build();
        }

        private static RemoteReporter BuildRemoteReporter(DeimosOptions options, ApplicationOptions appOptions, ILoggerFactory loggerFactory)
        {
            string host = string.IsNullOrEmpty(options.Jaeger.UdpHost) ? appOptions.KUBERNETES_NODE_IP : options.Jaeger.UdpHost;

            int port = options.Jaeger.UdpPort;
            DeimosLogger.JaegerLogger.UdpRemoteReporter(host, port);
            return new RemoteReporter.Builder()
                   .WithSender(new UdpSender(host, port, options.Jaeger.MaxPacketSize))
                   .WithLoggerFactory(loggerFactory)
                   .Build();
        }

        private static ISampler GetSampler(JaegerOptions options)
        {
            return options.Sampler switch
                   {
                       "const"         => new ConstSampler(true),
                       "rate"          => new RateLimitingSampler(options.MaxTracesPerSecond),
                       "probabilistic" => new ProbabilisticSampler(options.SamplingRate),
                       _               => new ConstSampler(true)
                   };
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
    }
}
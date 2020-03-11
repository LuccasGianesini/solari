using System;
using System.Collections.Generic;
using System.Globalization;
using Elastic.Apm;
using Elastic.Apm.AspNetCore;
using Elastic.Apm.AspNetCore.DiagnosticListener;
using Elastic.Apm.Config;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Solari.Deimos.Abstractions;
using Solari.Sol;

namespace Solari.Deimos.Elastic
{
    public static class ElasticTracerConfiguration
    {
        public static ISolariBuilder AddElasticApm(ISolariBuilder solariBuilder, DeimosOptions options)
        {
            solariBuilder.AddBuildAction(provider =>
            {
                var marshal = provider.GetRequiredService<ISolariMarshal>();
                ApplicationOptions appOptions = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                ElasticApmOptions opt = provider.GetRequiredService<IOptions<DeimosOptions>>().Value.Elastic;
                ConfigureViaEnvironmentVariables(opt, appOptions);
                marshal.ApplicationBuilder?.UseElasticApm(subscribers: ConfigureSubs(options));
                DeimosLogger.ElasticLogger.ApmServerAddress(opt.Url);
                DeimosLogger.ElasticLogger.UsingElasticApmTracer();
            });
            solariBuilder.Services.AddSingleton<IDeimosElasticTracer, DeimosElasticTracer>(provider => new DeimosElasticTracer(Agent.Tracer));
            return solariBuilder;
        }

        private static IDiagnosticsSubscriber[] ConfigureSubs(DeimosOptions options)
        {
            var subscribers = new List<IDiagnosticsSubscriber>();

            if (options.Elastic.Subscribers.Http)
            {
                DeimosLogger.ElasticLogger.UsingHttpSubscriber();
                subscribers.Add(new HttpDiagnosticsSubscriber());
            }

            if (options.Elastic.Subscribers.EfCore)
            {
                DeimosLogger.ElasticLogger.UsingEfCoreSubscriber();
                subscribers.Add(new EfCoreDiagnosticsSubscriber());
            }

            if (options.Elastic.Subscribers.AspNetCore)
            {
                DeimosLogger.ElasticLogger.UsingAspNetCoreSubscriber();
                subscribers.Add(new AspNetCoreDiagnosticsSubscriber());
            }
            
            return subscribers.ToArray();
        }
        private static void ConfigureViaEnvironmentVariables(ElasticApmOptions opt, ApplicationOptions appOptions)
        {
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.ServerUrls, opt.Url);
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.SecretToken, opt.SecretToken);
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.VerifyServerCert, opt.VerifyServerCert.ToString());
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.FlushInterval, opt.FlushInterval);
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.MaxBatchEventCount, opt.MaxBatchEventCount.ToString());
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.MaxQueueEventCount, opt.MaxQueueEventCount.ToString());
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.MetricsInterval, opt.MetricsInterval);
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.CaptureBody, opt.CaptureBody);
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.LogLevel, opt.LogLevel);
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.Environment, appOptions.ApplicationEnvironment);
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.TransactionSampleRate, opt.TransactionSampleRate.ToString(CultureInfo.InvariantCulture));
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.TransactionMaxSpans, opt.TransactionMaxSpans.ToString());
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.CaptureHeaders, opt.CaptureHeaders.ToString());
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.ServiceName, appOptions.ApplicationName);
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.ServiceNodeName, appOptions.ApplicationInstanceId);
            EnvVarHelper.TrySetValue(ConfigConsts.EnvVarNames.ServiceVersion, appOptions.ApplicationVersion);
            DeimosLogger.ElasticLogger.ConfiguredTracer();
        }
    }
}
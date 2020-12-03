using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;
using OpenTracing.Util;
using Solari.Deimos.Abstractions;
using Solari.Sol.Abstractions.Extensions;

namespace Solari.Deimos.MassTransit
{
    //COPIED FROM https://github.com/yesmarket/MassTransit.OpenTracing
    public class OpenTracingConsumeFilter : IFilter<ConsumeContext>
    {
        private readonly IConfiguration _configuration;

        public OpenTracingConsumeFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Probe(ProbeContext context)
        {
        }

        public async Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
        {
            DeimosOptions options = GetDeimosOptions();
            if (options is null || !options.Enabled)
                return;
            if (GlobalTracer.Instance is null)
                return;

            var operationName = $"Consuming Message: {context.DestinationAddress.GetExchangeName()}";
            ISpanBuilder spanBuilder;
            ITracer tracer = GlobalTracer.Instance;
            try
            {
                ISpanContext parentSpanCtx = ExtractTraceContext(tracer, ExtractMessageHeaders(context));

                spanBuilder = CreateSpan(tracer, operationName, parentSpanCtx);
            }
            catch (Exception e)
            {
                spanBuilder = GlobalTracer.Instance.BuildSpan(operationName)
                                          .WithTag(Tags.Error, true)
                                          .WithTag("message", e.Message)
                                          .WithTag("context", "Error while extracting message headers or creating the tracing span.")
                                          .WithTag("Type", e.GetType().ToString());
            }

            spanBuilder
                .WithTag("message.types", string.Join(", ", context.SupportedMessageTypes))
                .WithTag("source.host.transit.version", context.Host.MassTransitVersion)
                .WithTag("source.host.process.id", context.Host.ProcessId)
                .WithTag("source.host.framework.version", context.Host.FrameworkVersion)
                .WithTag("source.host.machine", context.Host.MachineName)
                .WithTag("input.address", context.ReceiveContext.InputAddress.ToString())
                .WithTag("destination.address", context.DestinationAddress?.ToString())
                .WithTag("source.address", context.SourceAddress?.ToString())
                .WithTag("initiator.id", context.InitiatorId?.ToString())
                .WithTag("message.id", context.MessageId?.ToString());

            using (IScope scope = spanBuilder.StartActive(true))
            {
                try
                {
                    await next.Send(context);
                }
                catch (Exception e)
                {
                    scope.Span.SetTag(Tags.Error, true);
                    scope.Span.SetTag("context", "Error while sending message through the masstransit pipe.");
                    scope.Span.Log(ExtractExceptionInfo(e));
                }
            }
        }

        private static ISpanBuilder CreateSpan(ITracer tracer, string operationName, ISpanContext parentSpanCtx)
        {
            ISpanBuilder spanBuilder;
            spanBuilder = tracer.BuildSpan(operationName);
            if (parentSpanCtx != null)
            {
                spanBuilder = spanBuilder.AsChildOf(parentSpanCtx);
            }

            return spanBuilder;
        }

        private static ISpanContext ExtractTraceContext(ITracer tracer, Dictionary<string, string> headers)
        {
            return tracer.Extract(BuiltinFormats.TextMap, new TextMapExtractAdapter(headers));
        }

        private static Dictionary<string, string> ExtractMessageHeaders(ConsumeContext context)
        {
            return context.Headers.GetAll().ToDictionary(pair => pair.Key, pair => pair.Value.ToString());
        }

        private static Dictionary<string, object> ExtractExceptionInfo(Exception e)
        {
            return new Dictionary<string, object>
            {
                {"Event", "ERROR"},
                {"Type", e.GetType()},
                {"Message", e.Message},
                {"Source", e.Source},
                {"Stack-Trace", e.StackTrace}
            };
        }

        private DeimosOptions GetDeimosOptions()
        {
            return _configuration.GetOptions<DeimosOptions>(DeimosConstants.TracingAppSettingsSection);
        }
    }
}

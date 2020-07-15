using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPipes;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Util;

namespace MassTransit.OpenTracing
{
    //COPIED FROM https://github.com/yesmarket/MassTransit.OpenTracing
    public class OpenTracingConsumeFilter : IFilter<ConsumeContext>
    {
        public void Probe(ProbeContext context)
        { }

        public async Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
        {
            string operationName = $"Consuming Message: {context.DestinationAddress.GetExchangeName()}";

            ISpanBuilder spanBuilder;

            try
            {
                Dictionary<string, string?> headers = context.Headers.GetAll().ToDictionary(pair => pair.Key,
                                                                                            pair => pair.Value.ToString());
                ISpanContext parentSpanCtx = GlobalTracer.Instance.Extract(BuiltinFormats.TextMap, new TextMapExtractAdapter(headers));

                spanBuilder = GlobalTracer.Instance.BuildSpan(operationName);
                if (parentSpanCtx != null)
                {
                    spanBuilder = spanBuilder.AsChildOf(parentSpanCtx);
                }
            }
            catch (Exception)
            {
                spanBuilder = GlobalTracer.Instance.BuildSpan(operationName);
            }

            spanBuilder
                .WithTag("message.types", string.Join(", ", context.SupportedMessageTypes))
                .WithTag("source.host.asstransit.version", context.Host.MassTransitVersion)
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
                await next.Send(context);
            }
        }
    }
}

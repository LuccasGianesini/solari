using System.Threading.Tasks;
using GreenPipes;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Util;

namespace MassTransit.OpenTracing
{
    //COPIED FROM https://github.com/yesmarket/MassTransit.OpenTracing
    public class OpenTracingPublishFilter : IFilter<PublishContext>
    {
        public void Probe(ProbeContext context)
        { }

        public async Task Send(PublishContext context, IPipe<PublishContext> next)
        {
            string operationName = $"Publishing Message: {context.DestinationAddress.GetExchangeName()}";

            ISpanBuilder spanBuilder = GlobalTracer.Instance.BuildSpan(operationName)
                                                   .AsChildOf(GlobalTracer.Instance.ActiveSpan.Context)
                                                   .WithTag("destination.address", context.DestinationAddress?.ToString())
                                                   .WithTag("source.address", context.SourceAddress?.ToString())
                                                   .WithTag("initiator.id", context.InitiatorId?.ToString())
                                                   .WithTag("message.id", context.MessageId?.ToString());

            using (IScope scope = spanBuilder.StartActive())
            {
                GlobalTracer.Instance.Inject(
                   GlobalTracer.Instance.ActiveSpan.Context,
                   BuiltinFormats.TextMap,
                   new MassTransitTextMapInjectAdapter(context));

                await next.Send(context);
            }
        }
    }
}

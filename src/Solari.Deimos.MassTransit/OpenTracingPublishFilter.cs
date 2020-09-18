using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Util;
using Solari.Deimos.Abstractions;
using Solari.Sol.Extensions;

namespace Solari.Deimos.MassTransit
{
    //COPIED FROM https://github.com/yesmarket/MassTransit.OpenTracing
    public class OpenTracingPublishFilter : IFilter<PublishContext>
    {
        private readonly IConfiguration _configuration;

        public OpenTracingPublishFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Probe(ProbeContext context)
        { }

        public async Task Send(PublishContext context, IPipe<PublishContext> next)
        {
            DeimosOptions options = GetDeimosOptions();
            if(options is null || !options.Enabled)
                return;

            if(GlobalTracer.Instance is null)
                return;

            var operationName = $"Publishing Message: {context.DestinationAddress.GetExchangeName()}";

            ISpanBuilder spanBuilder = GlobalTracer.Instance.BuildSpan(operationName)
                                                   .AsChildOf(GlobalTracer.Instance.ActiveSpan.Context)
                                                   .WithTag("destination.address", context.DestinationAddress?.ToString())
                                                   .WithTag("source.address", context.SourceAddress?.ToString())
                                                   .WithTag("initiator.id", context.InitiatorId?.ToString())
                                                   .WithTag("message.id", context.MessageId?.ToString());

            using (IScope scope = spanBuilder.StartActive())
            {
                GlobalTracer.Instance.Inject(GlobalTracer.Instance.ActiveSpan.Context,
                                             BuiltinFormats.TextMap, new MassTransitTextMapInjectAdapter(context));

                await next.Send(context);
            }
        }

        private DeimosOptions GetDeimosOptions() => _configuration.GetOptions<DeimosOptions>(DeimosConstants.TracingAppSettingsSection);
    }
}

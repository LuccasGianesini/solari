using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Solari.Deimos.MassTransit
{
    //COPIED FROM https://github.com/yesmarket/MassTransit.OpenTracing
    public static class OpenTracingMiddlewareConfiguratorExtensions
    {
        public static void PropagateOpenTracingContext(this IBusFactoryConfigurator value, IConfiguration configuration)
        {
            value.ConfigurePublish(configurator => configurator.AddPipeSpecification(new OpenTracingPipeSpecification(configuration)));
            value.AddPipeSpecification(new OpenTracingPipeSpecification(configuration));
        }
    }
}

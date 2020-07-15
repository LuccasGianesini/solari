namespace MassTransit.OpenTracing
{
    //COPIED FROM https://github.com/yesmarket/MassTransit.OpenTracing
    public static class OpenTracingMiddlewareConfiguratorExtensions
    {
        public static void PropagateOpenTracingContext(this IBusFactoryConfigurator value)
        {
            value.ConfigurePublish(configurator => configurator.AddPipeSpecification(new OpenTracingPipeSpecification()));
            value.AddPipeSpecification(new OpenTracingPipeSpecification());
        }
    }
}

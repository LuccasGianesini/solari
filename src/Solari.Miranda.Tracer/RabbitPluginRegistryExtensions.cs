using Convey.MessageBrokers.RabbitMQ;

namespace Solari.Miranda.Tracer
{
    public static class RabbitPluginRegistryExtensions
    {
        public static IRabbitMqPluginsRegistry AddElasticApm(this IRabbitMqPluginsRegistry registry)
        {
            registry.Add<ElasticPlugin>();
            return registry;
        }
    }
}
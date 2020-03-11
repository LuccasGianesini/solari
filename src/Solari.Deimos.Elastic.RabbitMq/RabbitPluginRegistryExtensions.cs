using Convey.MessageBrokers.RabbitMQ;

namespace Solari.Deimos.Elastic.RabbitMq
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
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Enrichers.MessageContext;
using Solari.Miranda.Abstractions;
using Solari.Miranda.Abstractions.Options;
using Solari.Miranda.Framework;
using Solari.Titan;

namespace Solari.Miranda
{
    public class MirandaClient
    {
        private readonly IBusClient _client;
        private readonly ITitanLogger<MirandaClient> _logger;
        private readonly IServiceProvider _provider;
        private readonly INamingConventions _namingConventions;
        private readonly IModel _connection;


        public MirandaClient(IBusClient client, ITitanLogger<MirandaClient> logger, IServiceProvider provider)
        {
            _client = client;
            _logger = logger;
            _provider = provider;
            _namingConventions = _provider.GetService<INamingConventions>();
            _connection = _provider.GetService<IConnection>().CreateModel();
        }


        public async Task PublishAsync<T>(T message, IMirandaMessageContext messageContext = null)
        {
            _logger.Debug($"Publishing message with id {messageContext?.MessageId}");
            await _client.PublishAsync(message, context => context.UseMessageContext(messageContext));
        }

        public Task SubscribeAsync<TMessage>(Func<IServiceProvider, TMessage, IMirandaMessageContext, Task> handle)
        {
            
        }


        public void PreSubscription<T>(MirandaOptions options)
        {
            string exchange = AttributeHelper.GetExchange(typeof(T), options.Namespace);
            string queue = AttributeHelper.GetQueueName(typeof(T).Assembly.GetName().Name, typeof(T), options.Namespace);
            string

        }

        public void ConfigureQueue(MirandaOptions options)
        {
            if (options.Queue.Declare)
            {
                
            }
            
        }
        
    }
}
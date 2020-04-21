using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.BasicPublish;
using RawRabbit.Configuration.Get;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Enrichers.Polly;
using RawRabbit.Pipe;
using Solari.Deimos.CorrelationId;
using Solari.Miranda.Abstractions;
using Solari.Miranda.Abstractions.Options;
using Solari.Miranda.Framework;
using Solari.Titan;
using Thrift.Protocols.Entities;

namespace Solari.Miranda
{
    public class MirandaClient : IMirandaClient
    {
        private readonly IBusClient _client;
        private readonly ITitanLogger<MirandaClient> _logger;
        private readonly IServiceProvider _provider;
        private readonly ICorrelationContextManager _manager;

        public MirandaClient(IBusClient client, ITitanLogger<MirandaClient> logger, IServiceProvider provider)
        {
            _client = client;
            _logger = logger;
            _provider = provider;
            _manager = _provider.GetService<ICorrelationContextManager>();
        }


        public async Task PublishAsync<T>(T message, IMirandaMessageContext messageContext = null)
        {
            if (messageContext == null)
            {
                _logger.Information("Message context not provided.");
                messageContext = CreateMirandaMessageContext();
            }

            _logger.Information($"Publishing message with id {messageContext?.MessageId}");
            await _client.PublishAsync(message, context => context.UseMessageContext(messageContext));
        }

        private IMirandaMessageContext CreateMirandaMessageContext()
        {
            if (_manager != null)
            {
                ICorrelationContext c = _manager.GetOrCreateAndSet();
                return new MirandaMessageContext
                {
                    Interval = 5,
                    Retries = 3,
                    EnvoyCorrelationContext = c.EnvoyCorrelationContext
                };
            }

            return new MirandaMessageContext
            {
                Empty = true
            };
        }

        public async Task SubscribeAsync<T>(Func<IServiceProvider, T, IMirandaMessageContext, Task> handle)
        {
            await _client.SubscribeAsync<T, IMirandaMessageContext>(async (message, context) =>
            {
                try
                {
                    var handler = _provider.GetService<ICorrelationContextAccessor>();
                    handler.Current = context;

                    Exception exception =
                        await TryHandleAsync(message, context, handle, context.Retries, context.Interval);

                    if (exception is null)
                        return new Ack();

                    throw exception;
                }
                catch (Exception e)
                {
                    _logger.Critical(e.Message, e);

                    throw;
                }
            });
        }

        private Task<Exception> TryHandleAsync<T>(T message, IMirandaMessageContext correlationContext,
                                                  Func<IServiceProvider, T, IMirandaMessageContext, Task> handle, int retries, double interval)
        {
            var currentRetry = 0;
            string messageName = AttributeHelper.GetMessageName(typeof(T));

            AsyncRetryPolicy retryPolicy = Policy
                                           .Handle<Exception>()
                                           .WaitAndRetryAsync(retries, i => TimeSpan.FromSeconds(interval));

            return retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    _logger.Information(PreLogMessage(messageName, correlationContext, currentRetry));

                    await handle(_provider, message, correlationContext);

                    _logger.Information(PostLogMessage(messageName, correlationContext, currentRetry));
                    return null;
                }
                catch (Exception ex)
                {
                    currentRetry++;

                    return await HandleException(message, correlationContext, ex, messageName);
                }
            });
        }

        private async Task<Exception> HandleException<T>(T message, IMirandaMessageContext correlationContext, Exception ex, string messageName)
        {
            _logger.Error(ex.Message, ex);

            await PublishRejectedEventMessage(message, correlationContext);

            _logger.Warning(RejectedLogMessage(messageName, ex.Message, correlationContext));

            return FiledMessageException(messageName, ex.Message, ex);
        }

        private async Task PublishRejectedEventMessage(object rejectedEvent, IMirandaMessageContext correlationContext)
        {
            await _client.PublishAsync(rejectedEvent, ctx => ctx.UseMessageContext(correlationContext));
        }

        public bool TyrGetContext(ICorrelationContextAccessor accessor, out ICorrelationContext context)
        {
            if (accessor == null)
            {
                context = null;
                return false;
            }

            context = accessor.Current;
            return true;
        }

        private static Exception FiledMessageException(string messageName, string rejectedEventMessage, Exception exception)
        {
            return new Exception($"Handling: '{messageName}' failed and rejected event: " +
                                 $"'{rejectedEventMessage}' was published.", exception);
        }

        private static string PostLogMessage(string messageName, IMirandaMessageContext correlationContext, int currentRetry)
        {
            return $"Handled: '{messageName}' " +
                   $"with request id: '{correlationContext?.EnvoyCorrelationContext.RequestId}'. {RetryMessage(currentRetry)}";
        }


        private static string PreLogMessage(string messageName, IMirandaMessageContext correlationContext, int currentRetry)
        {
            return $"Handling: '{messageName}' " +
                   $"with request id: '{correlationContext?.EnvoyCorrelationContext.RequestId}'. {RetryMessage(currentRetry)}";
        }

        private static string RejectedLogMessage(string messageName, string rejectedEventMessage, IMirandaMessageContext correlationContext)
        {
            return $"Published a rejected event: '{rejectedEventMessage}' " +
                   $"for the message: '{messageName}' with request id: '{correlationContext?.EnvoyCorrelationContext.RequestId}'.";
        }

        private static string RetryMessage(int currentRetry) { return currentRetry == 0 ? string.Empty : $"Retry: {currentRetry}'."; }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit.Pipe;
using RawRabbit.Pipe.Middleware;
using Solari.Miranda.Abstractions;

namespace Solari.Miranda.Framework
{
    internal class ProcessUniqueMessagesMiddleware : StagedMiddleware
    {
        private readonly IServiceProvider _serviceProvider;
        public override string StageMarker { get; } = global::RawRabbit.Pipe.StageMarker.MessageDeserialized;

        public ProcessUniqueMessagesMiddleware(IServiceProvider serviceProvider) { _serviceProvider = serviceProvider; }

        public override async Task InvokeAsync(IPipeContext context, CancellationToken token = new CancellationToken())
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var messageProcessor = scope.ServiceProvider.GetRequiredService<IMessageProcessor>();
                var messageId = context.GetDeliveryEventArgs().BasicProperties.MessageId;
                MirandaLogger.RedisMessageProcessor.LogReceivedUniqueMessage(messageId);
                if (!await messageProcessor.TryProcessAsync(messageId))
                {
                    MirandaLogger.RedisMessageProcessor.LogMessageWasAlreadyProcessed(messageId);
                    return;
                }

                try
                {
                    MirandaLogger.RedisMessageProcessor.LogProcessingUniqueMessage(messageId);
                    await Next.InvokeAsync(context, token);
                    MirandaLogger.RedisMessageProcessor.LogPrecessedUniqueMessage(messageId);
                }
                catch
                {
                    MirandaLogger.RedisMessageProcessor.LogErrorProcessingUniqueMessage(messageId);
                    await messageProcessor.RemoveAsync(messageId);
                    throw;
                }
            }
        }
    }
}
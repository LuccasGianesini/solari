using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using RawRabbit.Common;
using RawRabbit.Configuration.BasicPublish;
using RawRabbit.Configuration.Get;
using Solari.Miranda.Abstractions;

namespace Solari.Miranda
{
    public interface IMirandaClient
    {
        Task PublishAsync<T>(T message, IMirandaMessageContext messageContext = null);
        Task SubscribeAsync<T>(Func<IServiceProvider, T, MirandaMessageContext, Task> handle);
    }
}
using System;
using System.Threading.Tasks;
using Solari.Miranda.Abstractions;

namespace Solari.Miranda
{
    public interface IMirandaClient
    {
        Task PublishAsync<T>(T message, IMirandaMessageContext messageContext = null);
        Task SubscribeAsync<TMessage>(Func<IServiceProvider, TMessage, IMirandaMessageContext, Task> handle);
    }
}
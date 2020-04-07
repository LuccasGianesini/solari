using System.Threading.Tasks;

namespace Solari.Miranda.Framework
{
    public class EmptyMessageProcessor : IMessageProcessor
    {
        public Task<bool> TryProcessAsync(string id) => Task.FromResult(true);

        public Task RemoveAsync(string id) => Task.CompletedTask;
    }
}
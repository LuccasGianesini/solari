using System.Threading.Tasks;
using Solari.Miranda;
using Solari.Samples.Domain;

namespace Solari.Samples.Application
{
    public class MirandaPublisher : IMirandaPublisher
    {
        private readonly IMirandaClient _client;

        public MirandaPublisher(IMirandaClient client) { _client = client; }

        public async Task PublishTestMessage(TestMessage message)
        {
            await _client.PublishAsync(message);
        }
    }
}
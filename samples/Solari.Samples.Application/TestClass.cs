using System.Threading.Tasks;
using Solari.Miranda;
using Solari.Samples.Domain;
using Solari.Titan;

namespace Solari.Samples.Application
{
    public class TestClass
    {
        private readonly ITitanLogger<TestClass> _logger;
        private readonly IMirandaClient _client;

        public TestClass(ITitanLogger<TestClass> logger, IMirandaClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task TestSubscription()
        {
            await _client.SubscribeAsync<TestMessage>((provider, msg, context) =>
            {
                _logger.Information(msg.Value);
                return Task.CompletedTask;
            });
        }
    }
}
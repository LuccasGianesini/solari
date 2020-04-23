using System.Net.Http;
using System.Threading.Tasks;
using Solari.Ganymede;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Pipeline;
using Solari.Miranda;

namespace Solari.Samples.WorkerService
{
    public interface ITestClient
    {
        Task<string> Get();
    }

    public class TestClient : GanymedeClient<TestClient>, ITestClient
    {
        private readonly IMirandaClient _client;
        public TestClient(HttpClient httpClient, IGanymedeRequest<TestClient> request, IMirandaClient client) : base(httpClient, request) { _client = client; }

        public async Task<string> Get()
        {
            GanymedeHttpResponse s = await NewRequest.ForResource("Get").Send();
            return await s.AsString();
        }
    }
}
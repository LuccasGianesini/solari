using System.Net.Http;
using System.Threading.Tasks;
using Solari.Ganymede;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Pipeline;

namespace Solari.Samples.WorkerService
{
    public interface ITestClient
    {
        Task<string> Get();
    }

    public class TestClient : GanymedeClient<TestClient>, ITestClient
    {
        public TestClient(HttpClient httpClient, IGanymedeRequest<TestClient> request) : base(httpClient, request) { }

        public async Task<string> Get()
        {
            GanymedeHttpResponse s = await NewRequest.ForResource("Get").Send();
            return await s.AsString();
        }
    }
}
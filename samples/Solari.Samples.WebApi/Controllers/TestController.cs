using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Solari.Deimos.Abstractions;
using Solari.Hyperion.Abstractions;
using Solari.Samples.Domain;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Commands;
using Solari.Titan;

namespace Solari.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TestController : ControllerBase
    {
        private readonly IKeycloakClient _keycloakClient;
        private readonly IHyperionClient _client;
        private readonly IPersonCollection _collection;
        private readonly IGitHubClient _hubClient;
        private readonly ILogger<TestController> _logger;
        private readonly IPersonOperations _operations;

        public TestController(IKeycloakClient keycloakClient)
        {
            _keycloakClient = keycloakClient;
        }

        [HttpPost]
        public async Task<IActionResult> Get([FromBody] UpdatePersonDto dto)
        {
            await _keycloakClient.Signin(new AuthModel());


            return Ok();
        }

        public async Task<IActionResult> Error()
        {
            await _client.Kv.SaveToKv("test-data", new Person("OP"));
            IList<HyperionService> svc = await _client.Services.GetServiceAddresses("solari-samples-webapi");
            return Ok(svc);
        }
    }
}

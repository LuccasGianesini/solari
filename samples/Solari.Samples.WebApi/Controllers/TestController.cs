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
        private readonly IHyperionClient _client;
        private readonly IPersonRepository _repository;
        private readonly IGitHubClient _hubClient;
        private readonly ILogger<TestController> _logger;
        private readonly IPersonOperations _operations;

        public TestController(ILogger<TestController> logger,  IPersonOperations operations, IHyperionClient client, IPersonRepository repository)
        {
            _logger = logger;
            _operations = operations;
            _client = client;
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Get([FromBody] UpdatePersonDto dto)
        {
           await _repository.InsertPerson(_operations.CreateInsertOperation(new CreatePersonCommand
            {
                Name = "Test"
            }));
            
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
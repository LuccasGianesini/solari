using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elastic.CommonSchema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTracing.Tag;
using Solari.Deimos.Abstractions;
using Solari.Hyperion;
using Solari.Hyperion.Abstractions;
using Solari.Samples.Domain;
using Solari.Samples.Domain.Person;
using Solari.Titan;

namespace Solari.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TestController : ControllerBase
    {
        private readonly ITitanLogger<TestController> _logger;
        private readonly IDeimosTracer _tracer;
        private readonly IPersonOperations _operations;
        private readonly IHyperionClient _client;
        private readonly IGitHubClient _hubClient;

        public TestController(ITitanLogger<TestController> logger, IDeimosTracer tracer, IPersonOperations operations, IHyperionClient client)
        {
            _logger = logger;
            _tracer = tracer;
            _operations = operations;
            _client = client;
        }

        [HttpPost]
        public async Task<IActionResult> Get([FromBody] UpdatePersonDto dto)
        {
            _operations.CreateUpdatePersonOperation(dto.Id, dto);
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
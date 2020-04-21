using System.Collections.Generic;
using System.Threading.Tasks;
using Elastic.CommonSchema;
using Microsoft.AspNetCore.Mvc;
using Solari.Deimos.Abstractions;
using Solari.Samples.Domain;
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
        private readonly IGitHubClient _hubClient;

        public TestController(ITitanLogger<TestController> logger, IDeimosTracer tracer, IGitHubClient hubClient)
        {
            _logger = logger;
            _tracer = tracer;
            _hubClient = hubClient;
        }

        [HttpGet]
        public async  Task<IActionResult> Get()
        {
            _tracer.TraceOperation("test-operation");
            _logger.Information("Teste");
            _tracer.FinalizeTrace(new Dictionary<string, object>
            {
                {"Log", "jdalksndaljhdjlasjdlhasldkalhdlasjld"}
            });
            var prof = await _hubClient.GetUserProfile("LuccasGianesini");
          return  Ok(prof);
        } 

    }
}
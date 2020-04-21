using System.Collections.Generic;
using System.Threading.Tasks;
using Elastic.CommonSchema;
using Microsoft.AspNetCore.Mvc;
using Solari.Deimos.Abstractions;
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

        public TestController(ITitanLogger<TestController> logger, IDeimosTracer tracer)
        {
            _logger = logger;
            _tracer = tracer;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _tracer.TraceOperation("test-operation");
            _logger.Information("Teste");
            _tracer.FinalizeTrace(new Dictionary<string, object>
            {
                {"Log", "jdalksndaljhdjlasjdlhasldkalhdlasjld"}
            });
          return  Ok("1");
        } 

    }
}
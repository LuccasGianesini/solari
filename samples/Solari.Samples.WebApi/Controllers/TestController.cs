using System.Threading.Tasks;
using Elastic.CommonSchema;
using Microsoft.AspNetCore.Mvc;
using Solari.Titan;

namespace Solari.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TestController : ControllerBase
    {
        private readonly ITitanLogger<TestController> _logger;

        public TestController(ITitanLogger<TestController> logger) { _logger = logger; }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.Information("Teste");
          return  Ok("1");
        } 

    }
}
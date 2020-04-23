using System.Collections.Generic;
using System.Threading.Tasks;
using Elastic.CommonSchema;
using Microsoft.AspNetCore.Mvc;
using Solari.Deimos.Abstractions;
using Solari.Samples.Application;
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
        private readonly IMirandaSubscriber _subscriber;
        private readonly IMirandaPublisher _publisher;

        public TestController(ITitanLogger<TestController> logger, IDeimosTracer tracer, IGitHubClient hubClient, IMirandaSubscriber subscriber)
        {
            _logger = logger;
            _tracer = tracer;
            _hubClient = hubClient;
            _subscriber = subscriber;
            
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // string prof = await _hubClient.GetUserProfile("LuccasGianesini");
            await _subscriber.TestSubscription();
            // await _publisher.PublishTestMessage(new TestMessage{Value = "sadasdsadkasjdkals"});
            return Ok("1112");
        }
    }
}
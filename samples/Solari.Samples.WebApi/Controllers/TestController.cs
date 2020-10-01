using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Solari.Callisto;
using Solari.Callisto.Abstractions.Contracts;
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



        private readonly ILogger<TestController> _logger;
        private readonly ICallistoCollectionContext<Person> _context;

        public TestController(ILogger<TestController> logger, ICallistoCollectionContext<Person> context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Get([FromBody] UpdatePersonDto dto)
        {
            Person person = await _context.Insert(new Person("TESSTETESD"));


            IAsyncCursor<Person> cursor = await _context.Collection.FindAsync(Builders<Person>.Filter.Eq(a => a.Id, person.Id));
            Person result = await cursor.FirstOrDefaultAsync();

            return Ok(result);
        }

        public async Task<IActionResult> Error()
        {

            return Ok("svc");
        }
    }
}

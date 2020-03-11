using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Dtos;
using Solari.Sol;
using Solari.Vanth;

namespace Solari.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonApplication _application;
        private readonly ICommonResponseFactory _factory;

        public PersonController(IPersonApplication application, ICommonResponseFactory factory)
        {
            _application = application;
            _factory = factory;
        }

        [HttpPost]
        public async Task<IActionResult> Insert(InsertPersonDto dto)
        {
            if (dto == null)
            {
                return BadRequest(_factory.CreateError<Empty>(builder => builder
                                                                         .WithCode("005")
                                                                         .WithMessage("Provided object in null.")
                                                                         .Build()));
            }

            CommonResponse<PersonInsertedDto> result = await _application.InsertPerson(dto);
            if (result.HasResult)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
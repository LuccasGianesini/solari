using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Dtos;
using Solari.Samples.Domain.Person.Exceptions;
using Solari.Samples.Domain.Person.Results;
using Solari.Samples.Domain.Person.Validators;
using Solari.Sol;
using Solari.Titan;
using Solari.Vanth;

namespace Solari.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonApplication _application;
        private readonly ICommonResponseFactory _factory;
        private readonly ITitanLogger<PersonController> _logger;


        public PersonController(IPersonApplication application, ICommonResponseFactory factory, ITitanLogger<PersonController> logger)
        {
            _application = application;
            _factory = factory;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody]InsertPersonDto dto)
        {
            try
            {
                ValidationResult validationResult = new InsertPersonDtoValidator().Validate(dto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(_factory.CreateError<InsertPersonResult>(validationResult));
                }
                
                InsertPersonResult result = await _application.InsertPerson(dto);
                return Ok(_factory.CreateResult(result));
                
            }
            catch (MongoWriteException writeException)
            {
                _logger.Error("Error writing to database", writeException);
                return StatusCode(StatusCodes.Status500InternalServerError, _factory
                                      .CreateErrorFromException<InsertPersonResult>(writeException, "1001", "Error writing new person"));
            }
            catch (ArgumentNullException ag)
            {
                _logger.Error("Argument error", ag);
                return StatusCode(StatusCodes.Status500InternalServerError, _factory
                                      .CreateErrorFromException<InsertPersonResult>(ag, "1000", "A null argument was provided"));
            }

        }
    }
}
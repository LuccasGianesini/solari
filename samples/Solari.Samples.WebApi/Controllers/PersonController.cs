using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Commands;
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
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ICommonResponseFactory _factory;
        private readonly ITitanLogger<PersonController> _logger;


        public PersonController(ICommandDispatcher commandDispatcher, ICommonResponseFactory factory, ITitanLogger<PersonController> logger)
        {
            _commandDispatcher = commandDispatcher;
            _factory = factory;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody]CreatePersonRestIn input)
        {
            try
            {
                var command = new CreatePersonCommand(input.Name, input.Attributes);
                ValidationResult validationResult = new InsertPersonDtoValidator().Validate(command);
                if (!validationResult.IsValid)
                {
                    return BadRequest(_factory.CreateError<CreatePersonResult>(validationResult));
                }

                await _commandDispatcher.SendAsync(command);
                
                return Ok(_factory.CreateResult(command.Result));
                
            }
            catch (MongoWriteException writeException)
            {
                _logger.Error("Error writing to database", writeException);
                return StatusCode(StatusCodes.Status500InternalServerError, _factory
                                      .CreateErrorFromException<CreatePersonResult>(writeException, "1001", "Error writing new person"));
            }
            catch (ArgumentNullException ag)
            {
                _logger.Error("Argument error", ag);
                return StatusCode(StatusCodes.Status500InternalServerError, _factory
                                      .CreateErrorFromException<CreatePersonResult>(ag, "1000", "A null argument was provided"));
            }

        }
    }
}
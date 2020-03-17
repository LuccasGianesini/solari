using System;
using System.Reflection;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Solari.Samples.Domain;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Commands;
using Solari.Samples.Domain.Person.Exceptions;
using Solari.Samples.Domain.Person.Results;
using Solari.Samples.Domain.Person.Validators;
using Solari.Sol;
using Solari.Titan;
using Solari.Vanth;
using Solari.Vanth.Validation;

namespace Solari.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
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
        [VanthValidator]
        [ProducesResponseType(typeof(CommonResponse<CreatePersonResult>), 200)]
        [ProducesResponseType(typeof(CommonResponse<object>), 400)]
        [ProducesResponseType(typeof(CommonResponse<CreatePersonResult>), 500)]
        public async Task<IActionResult> Insert([FromBody] CreatePersonCommand command)
        {
            try
            {
                await _commandDispatcher.SendAsync(command);

                return Ok(_factory.CreateResult(command.Result));
            }
            catch (MongoWriteException exception)
            {
                return CreateExceptionError(exception, "1001", "Error writing new person", exception.GetType());
            }
            catch (ArgumentNullException exception)
            {
                return CreateExceptionError(exception, "1001", "Error writing new person", exception.GetType());
            }
        }

       

        [HttpPost("attributes")]
        [VanthValidator]
        public async Task<IActionResult> AddAttribute([FromBody] PersonAttributeCommand command)
        {
            try
            {
                await _commandDispatcher.SendAsync(command);
                return Ok(_factory.CreateResult(command.Result));
            }
            catch (MongoWriteException exception)
            {
                return CreateExceptionError(exception, "1002", "Error writing person attributes", exception.GetType());
            }
            catch (InvalidOperationException exception)
            {
                return CreateExceptionError(exception, "1002", "Error writing person attributes", exception.GetType());
            }
        }
        
        private IActionResult CreateExceptionError(Exception exception, string code, string message, MemberInfo exceptionType)
        {
            Helper.DefaultExceptionLogMessage(_logger, exception.GetType(), exception);
            return StatusCode(StatusCodes.Status500InternalServerError, _factory
                                  .CreateErrorFromException<CreatePersonResult>(exception, code, message));
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Solari.Eris;
using Solari.Samples.Domain;
using Solari.Samples.Domain.Person.Commands;
using Solari.Samples.Domain.Person.Results;
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
        private readonly IDispatcher _commandDispatcher;
        private readonly ICommonResponseFactory _factory;
        private readonly ILogger<PersonController> _logger;


        public PersonController(IDispatcher commandDispatcher, ICommonResponseFactory factory, ILogger<PersonController> logger)
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
                await _commandDispatcher.DispatchCommand(command);

                return Ok(_factory.CreateResult(command.Result));
            }
            catch (MongoWriteException exception)
            {
                return CreateExceptionError(exception, "1001", "Error writing new person");
            }
            catch (ArgumentNullException exception)
            {
                return CreateExceptionError(exception, "1001", "Error writing new person");
            }
        }


        [HttpPatch("attributes")]
        [VanthValidator]
        public async Task<IActionResult> PatchAttributes([FromBody] PersonAttributeCommand command)
        {
            try
            {
                await _commandDispatcher.DispatchCommand(command);
                return Ok(command.Result);
            }
            catch (MongoWriteException exception)
            {
                return CreateExceptionError(exception, "1002", "Error writing person attributes");
            }
            catch (InvalidOperationException exception)
            {
                return CreateExceptionError(exception, "1002", "Error writing person attributes");
            }
            catch (ArgumentOutOfRangeException exception)
            {
                return CreateExceptionError(exception, "1002", "Error writing person attributes");
            }
        }

        private IActionResult CreateExceptionError(Exception exception, string code, string message)
        {
            Helper.DefaultExceptionLogMessage(_logger, exception.GetType(), exception);
            return StatusCode(StatusCodes.Status500InternalServerError, _factory
                                  .CreateErrorFromException<CreatePersonResult>(exception, code, message));
        }
    }
}
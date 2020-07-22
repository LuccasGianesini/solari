using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Solari.Samples.Domain.Person.Commands;
using Solari.Samples.Domain.Person.Events;
using Solari.Samples.Domain.Person.Results;
using Solari.Vanth;
using Solari.Vanth.Validation;

namespace Solari.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PersonController : ControllerBase
    {
        private readonly IRequestClient<CreatePersonCommand> _createPersonClient;
        private readonly IResultFactory _factory;
        private readonly ILogger<PersonController> _logger;


        public PersonController()
        {

        }

        [HttpPost]
        [VanthValidator]
        [ProducesResponseType(typeof(Result<CreatePersonResult>), 200)]
        [ProducesResponseType(typeof(Result<object>), 400)]
        [ProducesResponseType(typeof(Result<CreatePersonResult>), 500)]
        public async Task<IActionResult> Insert([FromBody] CreatePersonCommand command)
        {
            try
            {
                Response<IPersonCreatedEvent> response =
                    await _createPersonClient.GetResponse<IPersonCreatedEvent>(command);

                return Ok(_factory.FromData(response.Message));
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
            return StatusCode(StatusCodes.Status500InternalServerError,
                              _factory.ExceptionError<CreatePersonResult>(exception,false ,code, message));
        }
    }
}

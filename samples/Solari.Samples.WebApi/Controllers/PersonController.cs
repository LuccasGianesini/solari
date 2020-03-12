using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Dtos;
using Solari.Samples.Domain.Person.Exceptions;
using Solari.Samples.Domain.Person.Results;
using Solari.Samples.Domain.Person.Validators;
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
            try
            {
               ValidationResult validationResult = new InsertPersonDtoValidator().Validate(dto);
               if (!validationResult.IsValid)
               {
                   return BadRequest(_factory.CreateError<InsertPersonResult>(validationResult));
               }
               return Ok(await _application.InsertPerson(dto));


            }
            catch (InsertPersonException insertPersonException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _factory
                                      .CreateErrorFromException<InsertPersonResult>(insertPersonException))
            }
            catch (ArgumentNullException ag)
            {
                
            }

          
        }
    }
}
using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Dtos;
using Solari.Samples.Domain.Person.Results;
using Solari.Titan;
using Solari.Vanth;

namespace Solari.Samples.Application
{
    public class PersonApplication : IPersonApplication
    {
        private readonly IPersonRepository _repository;
        private readonly IPersonOperations _operations;
        private readonly ICommonResponseFactory _factory;

        public PersonApplication(IPersonRepository repository, IPersonOperations operations,
                                 ICommonResponseFactory factory)
        {
            _repository = repository;
            _operations = operations;
            _factory = factory;
        }

        public async Task<InsertPersonResult> InsertPerson(InsertPersonDto dto)
        {
            ICallistoInsert<Person> op = _operations.CreateInsertOperation(dto);
            return await _repository.InsertPerson(op);
        }

        public async Task<CommonResponse<long>> AddAttributeToPerson(AddPersonAddAttributeDto dto)
        {
            try
            {
                // ICallistoUpdate<Person> op = _operations.CreateAddAttributeOperation(dto);
                // CommonResponse<InsertPersonResult> result = await _repository.AddAttribute(op);

                return new CommonResponse<long>().AddResult(long. MaxValue);
            }
            catch (ArgumentNullException ag)
            {
                return _factory.CreateErrorFromException<long>(ag, "003");
            }
        }
    }
}
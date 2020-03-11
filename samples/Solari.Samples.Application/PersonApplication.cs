using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Dtos;
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

        public async Task<CommonResponse<PersonInsertedDto>> InsertPerson(InsertPersonDto dto)
        {
            try
            {
                ICallistoInsert<Person> op = _operations.CreateInsertOperation(dto);
                return await _repository.InsertPerson(op);
            }
            catch (ArgumentNullException ag)
            {
                return _factory.CreateErrorFromException<PersonInsertedDto>(ag, false, errorCode: "001");
            }
        }

        public async Task<CommonResponse<long>> AddAttributeToPerson(PersonAddAttributeDto dto)
        {
            try
            {
                ICallistoUpdate<Person> op = _operations.CreateAddAttributeOperation(dto);
                CommonResponse<UpdateResult> result = await _repository.AddAttribute(op);
                return result.HasResult 
                           ? result.Transform(result.Result.ModifiedCount, false) 
                           : result.Transform(long.MinValue, true);
            }
            catch (ArgumentNullException ag)
            {
                return _factory.CreateErrorFromException<long>(ag, false, errorCode: "003");
            }
        }
    }
}

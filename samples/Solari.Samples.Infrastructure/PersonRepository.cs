using System;
using System.Threading.Tasks;
using Elastic.CommonSchema;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto;
using Solari.Callisto.Abstractions.CQR;
using Solari.Rhea;
using Solari.Samples.Domain;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Dtos;
using Solari.Sol;
using Solari.Vanth;

namespace Solari.Samples.Infrastructure
{
    public class PersonRepository : CallistoRepository<Person>, IPersonRepository
    {
        private readonly ICommonResponseFactory _factory;

        public PersonRepository(ICallistoContext context, ICommonResponseFactory factory) : base(context) { _factory = factory; }
        public async Task<CommonResponse<PersonInsertedDto>> InsertPerson(ICallistoInsert<Person> insert)
        {
            try
            {
                await Insert.One(insert);
                return _factory.CreateResult(PersonInsertedDto.Create(insert.Value.Id));
            }
            catch (MongoWriteException e)
            {
                return _factory.CreateErrorFromException<PersonInsertedDto>(e, errorCode: "002", message: "Error while insert a new person into the database");
            }
            
        }

        public async Task<CommonResponse<UpdateResult>> AddAttribute(ICallistoUpdate<Person> update)
        {
            try
            {
                UpdateResult result = await Update.One(update);
                return _factory.CreateResult(result);
            }
            catch (MongoWriteException e)
            {
                return _factory.CreateErrorFromException<UpdateResult>(e, errorCode: "003", message: "Error while adding an attribute to person");
            }
        }

        
    }
}
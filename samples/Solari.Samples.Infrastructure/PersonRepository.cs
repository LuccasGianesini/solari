using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Solari.Callisto;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Dtos;
using Solari.Samples.Domain.Person.Exceptions;
using Solari.Samples.Domain.Person.Results;
using Solari.Vanth;

namespace Solari.Samples.Infrastructure
{
    public class PersonRepository : CallistoRepository<Person>, IPersonRepository
    {
        private readonly ICommonResponseFactory _factory;

        public PersonRepository(ICallistoContext context, ICommonResponseFactory factory) : base(context) { _factory = factory; }
        public async Task<InsertPersonResult> InsertPerson(ICallistoInsert<Person> insert)
        {
            try
            {
                await Insert.One(insert);
                return InsertPersonResult.Create(insert.Value.Id);
            }
            catch (MongoWriteException e)
            {
                throw new InsertPersonException("Error while writing a new person", e);
            }
            
        }

        public async Task<UpdateResult> AddAttribute(ICallistoUpdate<Person> update)
        {
            try
            {
                return await Update.One(update);
            }
            catch (MongoWriteException e)
            {
                throw new AddPersonAttributeException("Error while writing attribute to person", e);
            }
        }

        
    }
}
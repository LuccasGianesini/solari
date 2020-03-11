using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using Solari.Callisto;
using Solari.Callisto.Abstractions.CQR;
using Solari.Samples.Domain;

namespace Solari.Samples.Infrastructure
{
    public class PersonRepository : CallistoRepository<Person>
    {
        public PersonRepository(ICallistoContext context) : base(context)
        {
        }
        public async Task<ObjectId> InsertPerson(ICallistoInsert<Person> insert)
        {
            Person inserted = await Insert.One(insert);
        }

        
    }
}
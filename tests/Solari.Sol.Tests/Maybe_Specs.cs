using Newtonsoft.Json;
using Solari.Sol.Abstractions;
using Solari.Sol.Tests.TestSetup;
using Xunit;

namespace Solari.Sol.Tests
{
    public class Maybe_Specs
    {
        private Person _person = new Person
        {
            Age = 22,
            Name = "BOLT"
        };
        // private const string _expectedJsonString = "{"Value":{"Name":"BOB","Age":22}}"

        [Fact]
        public void Should_Serialize_Only_The_Value_To_Json()
        {
            Maybe<Person> maybe = Maybe<Person>.Some(_person);
            string json = SolariJsonSerializer.New.SerializeToJson(maybe);
        }

    }
}

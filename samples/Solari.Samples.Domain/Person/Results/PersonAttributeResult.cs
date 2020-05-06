namespace Solari.Samples.Domain.Person.Results
{
    public class PersonAttributeResult
    {
        public PersonAttributeResult(long expected) { Expected = expected; }

        public long Expected { get; set; }
        public long Modified { get; set; }


        public bool IsSuccess() { return Expected == Modified; }
    }
}
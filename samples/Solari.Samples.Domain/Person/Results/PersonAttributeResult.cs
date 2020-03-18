namespace Solari.Samples.Domain.Person.Results
{
    public class PersonAttributeResult
    {
        public long Expected { get; set; }
        public long Modified { get; set; }
        

        public bool IsSuccess() => Expected == Modified;

        public PersonAttributeResult(long expected)
        {
            Expected = expected;
            
        }
    }
}
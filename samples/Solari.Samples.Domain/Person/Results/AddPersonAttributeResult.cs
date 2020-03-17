namespace Solari.Samples.Domain.Person.Results
{
    public class AddPersonAttributeResult
    {
        public bool Success { get; set; }
        public AddPersonAttributeResult(bool success) { Success = success; }
    }
}
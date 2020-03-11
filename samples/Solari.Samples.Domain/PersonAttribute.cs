using Solari.Callisto.Abstractions;

namespace Solari.Samples.Domain
{
    public class PersonAttribute : IDocumentNode
    {
        public string AttributeName { get; set; }
        
        public string AttributeValue { get; set; }
    }
}
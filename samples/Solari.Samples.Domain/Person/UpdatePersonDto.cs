using System.Collections.Generic;
using Solari.Callisto.Abstractions;

namespace Solari.Samples.Domain.Person
{
    public class UpdatePersonDto : ICallistoUpdate
    {
        public List<string> Fields { get; set; }
        
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
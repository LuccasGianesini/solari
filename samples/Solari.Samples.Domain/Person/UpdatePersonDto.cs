using System.Collections.Generic;
using Solari.Callisto.Abstractions;

namespace Solari.Samples.Domain.Person
{
    public class UpdatePersonDto : ICallistoUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<string> Fields { get; set; }
    }
}
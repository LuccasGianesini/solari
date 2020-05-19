using System.Collections;
using System.Collections.Generic;

namespace Solari.Deimos.CorrelationId
{
    public interface IUpdateDescriptorCollection
    {
        ICollection<UpdateDescriptor> Descriptors { get; set; }
    }
}
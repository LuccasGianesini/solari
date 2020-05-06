using Solari.Callisto.Abstractions;
using Solari.Callisto.Connector;

namespace Solari.Callisto
{
    public interface ICallistoContext
    {
        string CollectionName { get; }
        ICallistoOperationFactory OperationFactory { get; }
        ICallistoConnection Connection { get; }
    }
}
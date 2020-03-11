
using Solari.Callisto.Abstractions;
using Solari.Callisto.Connector;
using Solari.Callisto.Framework.Operators;

namespace Solari.Callisto
{
    public interface ICallistoContext
    {
        string CollectionName { get; }
        ICallistoOperationFactory OperationFactory { get; }
        ICallistoConnection Connection { get; }
    }
}
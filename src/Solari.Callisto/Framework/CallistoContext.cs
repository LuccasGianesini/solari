using Solari.Callisto.Abstractions;
using Solari.Callisto.Connector;

namespace Solari.Callisto.Framework
{
    public class CallistoContext : ICallistoContext
    {
        public CallistoContext(string collectionName, ICallistoConnection connection, ICallistoOperationFactory operationFactory)
        {
            CollectionName = collectionName;
            OperationFactory = operationFactory;
            Connection = connection;
        }

        public string CollectionName { get; }

        public ICallistoConnection Connection { get; }
        public ICallistoOperationFactory OperationFactory { get; }
    }
}
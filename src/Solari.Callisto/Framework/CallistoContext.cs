using System;
using Microsoft.Extensions.DependencyInjection;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Connector;

namespace Solari.Callisto.Framework
{
    public class CallistoContext : ICallistoContext
    {
        public string CollectionName { get; }

        public ICallistoConnection Connection { get; }
        public ICallistoOperationFactory OperationFactory { get; }

        public CallistoContext(string collectionName, ICallistoConnection connection, ICallistoOperationFactory operationFactory)
        {
            CollectionName = collectionName;
            OperationFactory = operationFactory;
            Connection = connection;
        }
    }
}
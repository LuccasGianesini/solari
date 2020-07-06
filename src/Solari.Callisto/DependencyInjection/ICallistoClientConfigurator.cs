using System;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;

namespace Solari.Callisto.DependencyInjection
{
    public interface ICallistoClientConfigurator
    {
        ICallistoClientConfigurator RegisterClient(string clientName, ICallistoClient client,
                                                   Action<ICallistoCollectionConfigurator> configureCollections);

        ICallistoClientConfigurator RegisterClient(string clientName, Action<ICallistoCollectionConfigurator> configureCollections);
    }
}

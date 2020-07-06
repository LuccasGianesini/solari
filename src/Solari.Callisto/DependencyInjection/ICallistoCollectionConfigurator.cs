using Microsoft.Extensions.DependencyInjection;

namespace Solari.Callisto.Abstractions
{
    public interface ICallistoCollectionConfigurator
    {
        /// <summary>
        ///     Register a MongoDb collection repository.
        /// </summary>
        /// <param name="collectionName">The collection name</param>
        /// <param name="lifetime">Lifetime of the repository service.</param>
        /// <typeparam name="TInterface">Repository interface</typeparam>
        /// <typeparam name="TConcrete">Repository Implementation</typeparam>
        /// <typeparam name="TDocumentRoot">Root document</typeparam>
        /// <returns></returns>
        ICallistoCollectionConfigurator ConfigureCollection<TInterface, TConcrete, TDocumentRoot>(string database,
                                                                                                  string collection,
                                                                                                  ServiceLifetime lifetime)
            where TDocumentRoot : class, IDocumentRoot
            where TConcrete : CallistoCollection<TDocumentRoot>, TInterface;

        ICallistoCollectionConfigurator ConfigureScopedCollection<TInterface, TConcrete, TDocumentRoot>(string database,
                                                                                                        string collection)
            where TDocumentRoot : class, IDocumentRoot
            where TConcrete : CallistoCollection<TDocumentRoot>, TInterface;

        ICallistoCollectionConfigurator ConfigureTransientCollection<TInterface, TConcrete, TDocumentRoot>(string database,
                                                                                                           string collection)
            where TDocumentRoot : class, IDocumentRoot
            where TConcrete : CallistoCollection<TDocumentRoot>, TInterface;

        ICallistoCollectionConfigurator ConfigureSingletonCollection<TInterface, TConcrete, TDocumentRoot>(string database,
                                                                                                           string collection)
            where TDocumentRoot : class, IDocumentRoot
            where TConcrete : CallistoCollection<TDocumentRoot>, TInterface;
    }
}

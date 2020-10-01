using MongoDB.Driver;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto
{
    public static class DocumentRootExtensions
    {
        public static FilterDefinition<T> FilterById<T>(this T document)
            where T : IDocumentRoot
        {
            return Builders<T>.Filter.Eq(a => a.Id, document.Id);
        }
    }
}

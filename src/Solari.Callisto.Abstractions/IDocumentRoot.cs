using MongoDB.Bson;

namespace Solari.Callisto.Abstractions
{
    public interface IDocumentRoot
    {
        ObjectId Id { get; set; }
    }
}
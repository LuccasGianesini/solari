using System.Net.Http;
using System.Threading.Tasks;
using Solari.Rhea;

namespace Solari.Ganymede.ContentSerializers
{
    public interface IContentDeserializer
    {
        Task<Maybe<TModel>> Deserialize<TModel>(HttpContent content);
    }
}
using System.Net.Http;
using System.Threading.Tasks;
using Solari.Sol.Utils;

namespace Solari.Ganymede.ContentSerializers
{
    public interface IContentDeserializer
    {
        Task<Maybe<TModel>> Deserialize<TModel>(HttpContent content);
    }
}
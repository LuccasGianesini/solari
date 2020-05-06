using System.Net.Http;
using System.Text;

namespace Solari.Ganymede.ContentSerializers
{
    public interface IContentSerializer
    {
        HttpContent Serialize(object content, string contentType = null, Encoding encoding = null);
    }
}
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Solari.Ganymede.ContentSerializers
{
    public interface IContentSerializer
    {
        HttpContent Serialize(object content, string contentType = null, Encoding encoding = null);
    }
}
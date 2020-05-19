using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Solari.Ganymede.Extensions
{
    internal static class HttpContentExtensions
    {
        public static HttpContent CloneContent(this HttpContent content)
        {
            if (content == null) return null;

            var ms = new MemoryStream();

            Task contentCloningTask =
                Task.Run(async () => await content
                                           .CopyToAsync(ms)
                                           .ConfigureAwait(false));

            contentCloningTask.Wait();

            if (!contentCloningTask.IsCompletedSuccessfully) return null;

            ms.Position = 0;
            var clone = new StreamContent(ms);
            foreach ((string key, IEnumerable<string> value) in content.Headers) clone.Headers.Add(key, value);

            return clone;
        }
    }
}
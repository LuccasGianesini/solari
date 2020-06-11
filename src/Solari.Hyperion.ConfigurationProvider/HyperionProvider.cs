using System.Net;
using System.Text;
using Consul;
using Newtonsoft.Json.Linq;
using Solari.Hyperion.Abstractions;
using Solari.Sol.Helpers;
using Solari.Sol.Utils;

namespace Solari.Hyperion.ConfigurationProvider
{
    public class HyperionProvider : Microsoft.Extensions.Configuration.ConfigurationProvider
    {
        private readonly HyperionConfigurationSource _source;

        public HyperionProvider(HyperionConfigurationSource source)
        {
            _source = source;
        }

        public override void Load()
        {
            Data.Clear();
            using (IConsulClient client = new ConsulClientFactory().Create(_source.Options))
            {

                string path = _source.ApplicationOptions.GetApplicationConfigurationPath("appsettings");
                // TODO Convert to ASYNC.
                // TODO Reload.
                QueryResult<KVPair> result = client.KV.Get(path)
                                                   .GetAwaiter()
                                                   .GetResult();
                if (result.StatusCode != HttpStatusCode.OK)
                    throw new
                        HyperionLoadException($"Error loading configurations from consul.HttpStatusCode: {result.StatusCode}. RequestTime: {result.RequestTime}");


                string json = Encoding.UTF8.GetString(result.Response.Value, 0, result.Response.Value.Length);
                if (string.IsNullOrEmpty(json))
                    throw new HyperionLoadException($"Could not find any configuration with the key {path}.");

                Data = new JsonParser().Parse(JObject.Parse(json));
            }
        }
    }
}

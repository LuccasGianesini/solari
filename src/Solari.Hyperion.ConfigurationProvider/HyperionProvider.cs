using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Consul;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Solari.Hyperion.Abstractions;
using Solari.Sol.Utils;

namespace Solari.Hyperion.ConfigurationProvider
{
    public class HyperionProvider : Microsoft.Extensions.Configuration.ConfigurationProvider
    {
        private readonly HyperionConfigurationSource _source;

        public HyperionProvider(HyperionConfigurationSource source) { _source = source; }

        public override void Load()
        {
            Data.Clear();
            using (IConsulClient client = new ConsulClientFactory().Create(_source.Options))
            {
                var key = BuildKey();
                QueryResult<KVPair> result = client.KV.Get(key).GetAwaiter().GetResult();
                if (result.StatusCode != HttpStatusCode.OK)
                    throw new
                        HyperionLoadException($"Error loading configurations from consul.HttpStatusCode: {result.StatusCode}. RequestTime: {result.RequestTime}");


                string json = Encoding.UTF8.GetString(result.Response.Value, 0, result.Response.Value.Length);
                if (string.IsNullOrEmpty(json))
                    throw new HyperionLoadException($"Could not find any configuration with the key {key}.");
                
                Data = new JsonParser().Parse(JObject.Parse(json));
            }
        }

        private string BuildKey()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(_source.Options.ConfigurationProvider.Path))
            {
                return sb.Append(_source.Options.ConfigurationProvider.Path).Append(_source.Options.ConfigurationProvider.ConfigurationFileName).ToString();
            }

            return sb.Append(_source.ApplicationOptions.ApplicationName).Append("/")
                     .Append(_source.ApplicationOptions.ApplicationEnvironment).Append("/")
                     .Append(_source.Options.ConfigurationProvider.ConfigurationFileName).ToString().ToLowerInvariant();
        }
    }
}
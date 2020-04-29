using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Options;
using Solari.Hyperion.Abstractions;
using Solari.Io;
using Solari.Sol;

namespace Solari.Hyperion
{
    public class KvOperations : IKvOperations
    {
        private readonly IConsulClient _client;
        private readonly ApplicationOptions _applicationOptions;

        public KvOperations(IConsulClient client, IOptions<ApplicationOptions> applicationOptions)
        {
            _client = client;
            _applicationOptions = applicationOptions.Value;
        }

        public async Task<WriteResult<bool>> DeleteFromKv(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new HyperionException("A key-value pair entry needs a key!");
            return await _client.KV.Delete(key);
        }

        public async Task<IReadOnlyList<string>> GetKeys(string prefix)
        {
            QueryResult<string[]> keys = await _client.KV.Keys(prefix);
            return keys.Response.ToList();
        }

        public async Task<WriteResult<bool>> SaveToKv<T>(string key, T value) where T : class
        {
            IsValid(key, value);
            return await _client.KV.Put(BuildKvPair(key, SerializeToJson(value)));
        }

        public async Task<WriteResult<bool>> SaveToKv(string key, bool value)
        {
            IsValid(key, value);
            return await _client.KV.Put(BuildKvPair(key, value.GetBytes()));
        }

        public async Task<WriteResult<bool>> SaveToKv(string key, string value)
        {
            IsValid(key, value);
            return await _client.KV.Put(BuildKvPair(key, value.GetBytes()));
        }

        public async Task<WriteResult<bool>> SaveToKv(string key, char value)
        {
            IsValid(key, value);
            return await _client.KV.Put(BuildKvPair(key, value.GetBytes()));
        }

        public async Task<WriteResult<bool>> SaveToKv(string key, int value)
        {
            IsValid(key, value);
            return await _client.KV.Put(BuildKvPair(key, value.GetBytes()));
        }

        public async Task<WriteResult<bool>> SaveToKv(string key, float value)
        {
            IsValid(key, value);
            return await _client.KV.Put(BuildKvPair(key, value.GetBytes()));
        }

        public async Task<WriteResult<bool>> SaveToKv(string key, double value)
        {
            IsValid(key, value);
            return await _client.KV.Put(BuildKvPair(key, value.GetBytes()));
        }

        public async Task<WriteResult<bool>> SaveToKv(string key, short value)
        {
            IsValid(key, value);
            return await _client.KV.Put(BuildKvPair(key, value.GetBytes()));
        }

        public async Task<WriteResult<bool>> SaveToKv(string key, decimal value)
        {
            IsValid(key, value);
            return await _client.KV.Put(BuildKvPair(key, value.GetBytes()));
        }

        public async Task<WriteResult<bool>> SaveToKv(string key, uint value)
        {
            IsValid(key, value);
            return await _client.KV.Put(BuildKvPair(key, value.GetBytes()));
        }

        public async Task<WriteResult<bool>> SaveToKv(string key, ulong value)
        {
            IsValid(key, value);
            return await _client.KV.Put(BuildKvPair(key, value.GetBytes()));
        }

        public async Task<WriteResult<bool>> SaveToKv(string key, ushort value)
        {
            IsValid(key, value);
            return await _client.KV.Put(BuildKvPair(key, value.GetBytes()));
        }

        public async Task<short> GetShortFromKv(string key)
        {
            QueryResult<KVPair> result = await _client.KV.Get(key);
            return result.Response.Value.ToShort();
        }


        public async Task<T> GetObjectFromKv<T>(string key)
        {
            QueryResult<KVPair> result = await _client.KV.Get(BuildKey(key));

            return SerializeToObject<T>(Encoding.UTF8.GetString(result.Response.Value, 0, result.Response.Value.Length));
        }

        public async Task<bool> GetBoolFromKv(string key)
        {
            QueryResult<KVPair> result = await _client.KV.Get(BuildKey(key));
            return result.Response.Value.ToBool();
        }

        public async Task<string> GetStringFromKv(string key)
        {
            QueryResult<KVPair> result = await _client.KV.Get(BuildKey(key));
            return Encoding.UTF8.GetString(result.Response.Value, 0, result.Response.Value.Length);
        }

        public async Task<char> GetCharFromKv(string key)
        {
            QueryResult<KVPair> result = await _client.KV.Get(BuildKey(key));
            return result.Response.Value.ToChar();
        }

        public async Task<int> GetIntFromKv(string key)
        {
            QueryResult<KVPair> result = await _client.KV.Get(BuildKey(key));
            return result.Response.Value.ToInt();
        }

        public async Task<float> GetFloatFromKv(string key)
        {
            QueryResult<KVPair> result = await _client.KV.Get(BuildKey(key));
            return result.Response.Value.ToFloat();
        }

        public async Task<double> GetDoubleFromKv(string key)
        {
            QueryResult<KVPair> result = await _client.KV.Get(BuildKey(key));
            return result.Response.Value.ToDouble();
        }

        public async Task<decimal> GetDecimalFromKv(string key)
        {
            QueryResult<KVPair> result = await _client.KV.Get(BuildKey(key));
            return result.Response.Value.ToDecimal();
        }

        public async Task<uint> GetUIntFromKv(string key)
        {
            QueryResult<KVPair> result = await _client.KV.Get(BuildKey(key));
            return result.Response.Value.ToUInt();
        }

        public async Task<ulong> GetULongFromKv(string key)
        {
            QueryResult<KVPair> result = await _client.KV.Get(BuildKey(key));
            return result.Response.Value.ToULong();
        }

        public async Task<ushort> GetUShortFromKv(string key)
        {
            QueryResult<KVPair> result = await _client.KV.Get(BuildKey(key));
            return result.Response.Value.ToUShort();
        }

        private static KVPair BuildKvPair(string key, byte[] value)
        {
            return new KVPair(key)
            {
                Value = value
            };
        }


        private KVPair BuildKvPair(string key, string value)
        {
            return new KVPair(BuildKey(key))
            {
                Value = Encoding.UTF8.GetBytes(value)
            };
        }

        private string BuildKey(string key) { return _applicationOptions.ApplicationName + "/" + key; }

        private static string SerializeToJson<T>(T @object) => JsonSerializer.Serialize(@object);
        private static T SerializeToObject<T>(string json) => JsonSerializer.Deserialize<T>(json);

        private static void IsValid(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                throw new HyperionException("A key-value pair entry needs a key!");
            if (value is null)
                throw new HyperionException("Cannot put a null object into kv store");
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solari.Juno.Abstractions;
using VaultSharp;
using VaultSharp.V1.Commons;

namespace Solari.Juno
{
    public class JunoClient : IJunoClient
    {
        public IVaultClient VaultClient { get; }
        public string ApplicationSecretsPath { get; }

        public JunoClient(IVaultClient client, string applicationSecretsPath)
        {
            VaultClient = client;
            ApplicationSecretsPath = applicationSecretsPath;
        }

        public async Task<IDictionary<string, object>> GetAppSettingsSecrets(string mountPoint = "kv")
        {
            return await GetSecretAsync("appsettings", mountPoint);
        }

        public async Task<T> GetAsync<T>(string key, string mountPoint = "kv") =>
            JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(await GetSecretAsync(key, mountPoint)));

        public async Task PutSecretAsync<T>(string key, T value, string mountPoint = "kv")
        {
            if (string.IsNullOrEmpty(key)) throw new JunoException("Cannot write to Vault with an empty key!");
            if (value == null) throw new JunoException("Null values cannot be written to Vault. ");
            await VaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(BuildApplicationSecretsKey(key), value, mountPoint: mountPoint);
        }

        public async Task<IDictionary<string, object>> GetSecretAsync(string key, string mountPoint)
        {
            if (string.IsNullOrEmpty(key))
                throw new JunoException("Cannot query vault with an empty key!");
            try
            {
                Secret<SecretData> secret = await VaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(BuildApplicationSecretsKey(key), mountPoint: mountPoint);
                return secret.Data.Data;
            }
            catch (Exception e)
            {
                throw new JunoException($"Getting Vault secret for key: '{key}' caused an error. {e.Message}", e, key);
            }
        }

        private string BuildApplicationSecretsKey(string key) => ApplicationSecretsPath + "/" + key;
    }
}
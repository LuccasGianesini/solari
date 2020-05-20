using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp;

namespace Solari.Juno.Abstractions
{
    public interface IJunoClient
    {
        IVaultClient VaultClient { get; }
        string ApplicationSecretsPath { get; }
        Task<IDictionary<string, object>> GetAppSettingsSecrets(string mountPoint = "kv");
        Task<T> GetAsync<T>(string key, string mountPoint = "kv");
        Task PutSecretAsync<T>(string key, T value, string mountPoint = "kv");
        Task<IDictionary<string, object>> GetSecretAsync(string key, string mountPoint);
    }
}
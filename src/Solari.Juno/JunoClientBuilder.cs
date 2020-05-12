using System;
using Solari.Juno.Abstractions;
using Solari.Sol;
using VaultSharp;
using VaultSharp.V1.AuthMethods;

namespace Solari.Juno
{
    public class JunoClientBuilder : IJunoClientBuilder
    {
        private readonly ApplicationOptions _options;
        private string _addr;
        private string _path;
        private IAuthMethodInfo _auth;

        public JunoClientBuilder(ApplicationOptions options) { _options = options; }

        public IJunoClientBuilder WithAddress(string address)
        {
            _addr = address;
            return this;
        }

        public IJunoClientBuilder WithAuthMethod(IAuthMethodInfo authMethodInfo)
        {
            _auth = authMethodInfo;
            return this;
        }

        public IJunoClientBuilder WithSecretsPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Vault application path cannot be null or empty", nameof(path));
            _path = path;
            return this;
        }

        public IJunoClient Build()
        {
            if (string.IsNullOrEmpty(_addr)) throw new ArgumentException("Vault address cannot be null.", nameof(_addr));
            if (_auth == null) throw new ArgumentNullException(nameof(_auth), "AuthMethod cannot be null");

            if (string.IsNullOrEmpty(_path))
            {
                if (_options != null)
                {
                    _path = _options.ApplicationName + "/" + _options.ApplicationEnvironment;
                }
            }

            var vClient = new VaultClient(new VaultClientSettings(_addr, _auth));
            return new JunoClient(vClient, _path);
        }
    }
}
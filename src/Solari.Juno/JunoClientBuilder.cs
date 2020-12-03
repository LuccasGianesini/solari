using System;
using Solari.Juno.Abstractions;
using Solari.Sol;
using Solari.Sol.Abstractions;
using VaultSharp;
using VaultSharp.V1.AuthMethods;

namespace Solari.Juno
{
    public class JunoClientBuilder : IJunoClientBuilder
    {
        private readonly ApplicationOptions _options;
        private string _addr;
        private IAuthMethodInfo _auth;

        public JunoClientBuilder(ApplicationOptions options)
        {
            _options = options;
        }

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


        public IJunoClient Build()
        {
            if (string.IsNullOrEmpty(_addr))
                throw new JunoException($"Vault address cannot be null.{nameof(IJunoClientBuilder.WithAddress)} ");
            if (_auth == null)
                throw new JunoException($"AuthMethod cannot be null. Please call {nameof(IJunoClientBuilder.WithAuthMethod)}");

            var vClient = new VaultClient(new VaultClientSettings(_addr, _auth));
            return new JunoClient(vClient, _options);
        }
    }
}

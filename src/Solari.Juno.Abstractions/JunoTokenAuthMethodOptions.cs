using System;

namespace Solari.Juno.Abstractions
{
    public class JunoTokenAuthMethodOptions
    {
        public string Token { get; set; } = Environment.GetEnvironmentVariable(JunoConstants.VAULT_TOKEN);
    }
}
using System;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.Token;

namespace Solari.Juno.Abstractions
{
    public class JunoOptions
    {
        public string Address { get; set; } = Environment.GetEnvironmentVariable(JunoConstants.VAULT_ADDR);
        public string AuthMethod { get; set; }

        public JunoConfigurationProviderOptions ConfigurationProvider { get; set; }
        public JunoAppRoleAuthMethodOptions AppRole { get; set; }
        public JunoTokenAuthMethodOptions Token { get; set; }

        public IAuthMethodInfo GetAuthMethodInfo()
        {
            if (string.IsNullOrEmpty(AuthMethod))
                return BuildAppRoleAuthMethodInfo();
            return AuthMethod.ToLowerInvariant() switch
                   {
                       "approle" => BuildAppRoleAuthMethodInfo(),
                       "token"   => BuildTokenAuthMethodInfo(),
                       _         => BuildAppRoleAuthMethodInfo()
                   };
        }

        private IAuthMethodInfo BuildTokenAuthMethodInfo()
        {
            Token ??= new JunoTokenAuthMethodOptions();
            if (string.IsNullOrEmpty(Token.Token))
                throw new JunoAuthException("Authentication token not provided. You can provide the value using VAULT_TOKEN environment variable");
            return new TokenAuthMethodInfo(Token.Token);
        }

        private IAuthMethodInfo BuildAppRoleAuthMethodInfo()
        {
            AppRole ??= new JunoAppRoleAuthMethodOptions();
            if (string.IsNullOrEmpty(AppRole.RoleId))
                throw new
                    JunoAuthException("AppRole RoleId not provided. Unable to authenticate. You can provide the value using VAULT_ROLE_ID environment variable.");
            if (string.IsNullOrEmpty(AppRole.SecretId))
                throw new
                    JunoAuthException("AppRole SecretId not provided. Unable to authenticate. You can provide the value using VAULT_SECRET_ID environment variable.");
            return new AppRoleAuthMethodInfo(AppRole.RoleId, AppRole.SecretId);
        }
    }
}

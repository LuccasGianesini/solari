using System;
using System.Security;

namespace Solari.Juno.Abstractions
{
    public class JunoAppRoleAuthMethodOptions
    {
        public string RoleId { get; set; } = Environment.GetEnvironmentVariable(JunoConstants.VAULT_ROLE_ID);
        public string SecretId { get; set; } = Environment.GetEnvironmentVariable(JunoConstants.VAULT_SECRET_ID);
    }
}
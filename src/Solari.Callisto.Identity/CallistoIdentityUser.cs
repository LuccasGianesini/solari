using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto.Identity
{
    // COPIED FROM https://github.com/matteofabbri/AspNetCore.Identity.Mongo
    public class CallistoIdentityUser : IdentityUser<Guid>, IDocumentRoot
    {
        public CallistoIdentityUser()
        {
            Roles = new List<string>();
            Claims = new List<IdentityUserClaim<string>>();
            Logins = new List<IdentityUserLogin<string>>();
            Tokens = new List<IdentityUserToken<string>>();
            RecoveryCodes = new List<TwoFactorRecoveryCode>();

        }

        public CallistoIdentityUser(string userName) : base(userName)
        {
            UserName = userName;
            NormalizedUserName = userName.ToUpperInvariant();
            Roles = new List<string>();
            Claims = new List<IdentityUserClaim<string>>();
            Logins = new List<IdentityUserLogin<string>>();
            Tokens = new List<IdentityUserToken<string>>();
            RecoveryCodes = new List<TwoFactorRecoveryCode>();
        }

        public string AuthenticatorKey { get; set; }

        public List<string> Roles { get; set; }

        public List<IdentityUserClaim<string>> Claims { get; set; }

        public List<IdentityUserLogin<string>> Logins { get; set; }

        public List<IdentityUserToken<string>> Tokens { get; set; }

        public List<TwoFactorRecoveryCode> RecoveryCodes { get; set; }
    }
}

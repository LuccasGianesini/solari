using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto.Identity
{
    // COPIED FROM https://github.com/matteofabbri/AspNetCore.Identity.Mongo
    public class CallistoIdentityRole : IdentityRole<Guid>, IDocumentRoot
    {
        public CallistoIdentityRole()
        {
            Claims = new List<IdentityRoleClaim<string>>();
        }

        public CallistoIdentityRole(string name)
        {
            Claims = new List<IdentityRoleClaim<string>>();
            Name = name;
            NormalizedName = name.ToUpperInvariant();

        }
        public override string ToString()
        {
            return Name;
        }

        public List<IdentityRoleClaim<string>> Claims { get; set; }
    }
}

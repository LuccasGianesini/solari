using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace Solari.Callisto.Identity
{
    public class MongoRole : IdentityRole<ObjectId>
    {
        public MongoRole()
        {
            Claims = new List<IdentityRoleClaim<string>>();
        }

        public MongoRole(string name) : this()
        {
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
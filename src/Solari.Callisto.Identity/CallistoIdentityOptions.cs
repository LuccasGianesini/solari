namespace Solari.Callisto.Identity
{
    // COPIED FROM https://github.com/matteofabbri/AspNetCore.Identity.Mongo
    public class CallistoIdentityOptions
    {
        public string RolesCollection { get; set; } = "roles";
        public string UsersCollection { get; set; } = "users";
        public string Database { get; set; }
        public string ClientName { get; set; }
    }
}

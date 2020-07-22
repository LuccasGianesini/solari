namespace Solari.Callisto.Identity
{
    // COPIED from https://github.com/matteofabbri/AspNetCore.Identity.Mongo
	public class CallistoIdentityOptions
	{
		public string ConnectionString { get; set; }

	    public string UsersCollection { get; set; } = "Users";

	    public string RolesCollection { get; set; } = "Roles";

	    public bool UseDefaultIdentity { get; set; } = true;
        public string ClientName { get; set; }
        public string Database { get; set; }
    }
}

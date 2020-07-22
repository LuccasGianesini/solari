namespace Solari.Callisto.Identity
{
	public class CallistoIdentityOptions
	{
		public string ConnectionString { get; set; } = "mongodb://localhost/default";

	    public string UsersCollection { get; set; } = "Users";

	    public string RolesCollection { get; set; } = "Roles";

	    public bool UseDefaultIdentity { get; set; } = true;
        public string ClientName { get; set; }
        public string Database { get; set; }
    }
}

namespace Solari.Samples.Domain
{
    public class AuthModel
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; } = "password";
        public string scope { get; set; } = "openid";
        public string username { get; set; }
        public string password { get; set; }
    }
}

namespace Solari.Miranda.Abstractions.Options
{
    public class MirandaSslOptions
    {
        public bool Enabled { get; set; }
        public string ServerName { get; set; }
        public string CertificatePath { get; set; }
    }
}
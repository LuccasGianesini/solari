namespace Solari.Titan.Abstractions
{
    public class FileOptions
    {
        public bool Enabled { get; set; }
        public string Path { get; set; }
        public string Period { get; set; } = "s10";
        public string RollingInterval { get; set; }
        public bool UseContentRoot { get; set; } = true;
    }
}
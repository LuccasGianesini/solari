namespace Solari.Ceres.Abstractions
{
    public class CpuUsageOptions
    {
        public bool Enabled { get; set; }
        public string Interval { get; set; } = "s10";
    }
}
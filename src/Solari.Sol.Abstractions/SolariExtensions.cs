namespace Solari.Sol.Abstractions
{
    public static class SolariExtensions
    {
        public static string GetApplicationConfigurationPath(this ApplicationOptions applicationOptions, string key)
        {
            return (GetApplicationConfigurationBasePath(applicationOptions) + "/" + key).ToLowerInvariant();
        }

        private static string GetApplicationConfigurationBasePath(ApplicationOptions applicationOptions)
            => (applicationOptions.ApplicationName + "/" + applicationOptions.ApplicationEnvironment);
    }
}

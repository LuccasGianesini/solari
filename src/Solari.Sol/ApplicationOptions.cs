using System;
using System.Buffers;
using System.Reflection;

namespace Solari.Sol
{
    public class ApplicationOptions
    {
        /// <summary>
        /// The instance id of the application.
        /// </summary>
        public string ApplicationInstanceId { get; } = $"{Guid.NewGuid():N}";

        /// <summary>
        /// The application name.
        /// </summary>
        public string ApplicationName { get; set; } = Assembly.GetEntryAssembly()?.GetName(false).Name.ToLowerInvariant().Replace(".", "-");

        /// <summary>
        /// The version of the application
        /// </summary>
        public string ApplicationVersion { get; set; }

        /// <summary>
        /// The HostIp of the application. If it's running in a k8s environment.
        /// </summary>
        public string KubernetesNodeIp { get; } = Environment.GetEnvironmentVariable(SolariConstants.K8S_NODE_HOSTIP);
        /// <summary>
        /// The environment that the application is running.
        /// </summary>
        public string ApplicationEnvironment => GetEnvironment();

        private string AspNetCoreEnvironment { get; } = Environment.GetEnvironmentVariable(SolariConstants.ASPNETCORE_ENVIRONMENT);
        private string DotNetEnvironment { get; } = Environment.GetEnvironmentVariable(SolariConstants.DOTNET_ENVIRONMENT);



        private string GetEnvironment()
        {
            if (!string.IsNullOrEmpty(AspNetCoreEnvironment))
                return AspNetCoreEnvironment;
            return !string.IsNullOrEmpty(DotNetEnvironment) ? DotNetEnvironment : "";
        }
    }
}
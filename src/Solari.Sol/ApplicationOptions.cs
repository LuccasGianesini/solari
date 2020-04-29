using System;
using System.Buffers;
using System.Collections.Generic;
using System.Reflection;
using MassTransit;

namespace Solari.Sol
{
    public class ApplicationOptions
    {
        /// <summary>
        /// The instance id of the application.
        /// </summary>
        public string ApplicationInstanceId { get; } = $"{NewId.Next()}";

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
        public string KUBERNETES_NODE_IP { get; } = Environment.GetEnvironmentVariable(SolariConstants.K8S_NODE_IP_ADDR);
        /// <summary>
        /// The environment that the application is running.
        /// </summary>
        public string ApplicationEnvironment => GetEnvironment();

        public string ApplicationId => GetApplicationId();

        public List<string> Tags { get; set; } = new List<string>();

        public bool IsInDevelopment() { return ApplicationEnvironment.ToLowerInvariant().Equals("development"); }
        
        private string GetApplicationId()
        {
            return ApplicationName + "-" + ApplicationInstanceId;
        }
        private string AspNetCoreEnvironment { get; } = Environment.GetEnvironmentVariable(SolariConstants.ASPNETCORE_ENVIRONMENT);
        private string DotNetEnvironment { get; } = Environment.GetEnvironmentVariable(SolariConstants.DOTNET_ENVIRONMENT);
        private string GetEnvironment()
        {
            if (!string.IsNullOrEmpty(AspNetCoreEnvironment))
                return AspNetCoreEnvironment;
            return !string.IsNullOrEmpty(DotNetEnvironment) ? DotNetEnvironment : "Production";
        }

     
    }
}
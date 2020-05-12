using System;
using System.Collections.Generic;
using System.Reflection;
using MassTransit;
using Solari.Sol.Extensions;

namespace Solari.Sol
{
    public class ApplicationOptions
    {
        /// <summary>
        ///     The instance id of the application.
        /// </summary>
        public string ApplicationInstanceId { get; } = $"{NewId.Next()}";

        /// <summary>
        ///     The application name.
        /// </summary>
        public string ApplicationName { get; set; } = (Assembly.GetEntryAssembly()?.GetName(false).Name?.Replace(".", "-"))?.ToLowerInvariant();

        /// <summary>
        ///     The version of the application
        /// </summary>
        public string ApplicationVersion { get; set; }

        /// <summary>
        ///     The environment that the application is running.
        /// </summary>
        public string ApplicationEnvironment => GetEnvironment().ToLowerInvariant();

        public string ApplicationId => GetApplicationId();

        public List<string> Tags { get; set; } = new List<string>();
        private string AspNetCoreEnvironment { get; } = Environment.GetEnvironmentVariable(SolariConstants.ASPNETCORE_ENVIRONMENT);
        private string DotNetEnvironment { get; } = Environment.GetEnvironmentVariable(SolariConstants.DOTNET_ENVIRONMENT);

        public bool IsInDevelopment() { return ApplicationEnvironment.ToLowerInvariant().Equals("development"); }

        private string GetApplicationId() { return ApplicationName + "-" + ApplicationInstanceId; }

        private string GetEnvironment()
        {
            if (!string.IsNullOrEmpty(AspNetCoreEnvironment))
                return AspNetCoreEnvironment;
            return !string.IsNullOrEmpty(DotNetEnvironment) ? DotNetEnvironment : "Production";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using MassTransit;
using Solari.Sol.Extensions;

// ReSharper disable UnusedMember.Global

namespace Solari.Sol
{
    public class ApplicationOptions
    {
        private string _appName;
        private string _appEnv;
        private string _appInstanceId;
        private string _appId;

        /// <summary>
        ///     The instance id of the application.
        /// </summary>
        public string ApplicationInstanceId
        {
            get
            {
                if (string.IsNullOrEmpty(_appInstanceId))
                {
                    _appInstanceId = $"{NewId.Next()}";
                }

                return _appInstanceId;
            }
        }

        /// <summary>
        ///     The application name.
        /// </summary>
        public string ApplicationName {
            get => _appName;
            set
            {
                if(string.IsNullOrEmpty(value))
                    throw new SolException("The application name was not provided.");
                _appName = value.ToKebabCase();
            }
        }


        public string Project { get; set; }

        /// <summary>
        ///     The version of the application
        /// </summary>
        public string ApplicationVersion { get; set; }

        /// <summary>
        ///     The environment that the application is running.
        /// </summary>
        public string ApplicationEnvironment
        {
            get
            {
                if (string.IsNullOrEmpty(_appEnv))
                {
                    _appEnv = GetEnvironment().ToLowerInvariant();
                }

                return _appEnv;
            }
        }

        public string ApplicationId
        {
            get
            {
                if (string.IsNullOrEmpty(_appId))
                {
                    _appId = GetApplicationId();
                }

                return _appId;
            }
        }

        public List<string> Tags { get; set; } = new List<string>();
        private string AspNetCoreEnvironment { get; } = Environment.GetEnvironmentVariable(SolariConstants.ASPNETCORE_ENVIRONMENT);
        private string DotNetEnvironment { get; } = Environment.GetEnvironmentVariable(SolariConstants.DOTNET_ENVIRONMENT);

        public bool IsInDevelopment() { return ApplicationEnvironment.Equals("development"); }

        private string GetApplicationId() { return ApplicationName + "-" + ApplicationInstanceId; }

        private string GetEnvironment()
        {
            if (!string.IsNullOrEmpty(AspNetCoreEnvironment))
                return AspNetCoreEnvironment;
            return !string.IsNullOrEmpty(DotNetEnvironment) ? DotNetEnvironment : "Production";
        }
    }
}

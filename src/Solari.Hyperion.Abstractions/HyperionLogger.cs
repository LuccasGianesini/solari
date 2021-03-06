﻿using Serilog;

namespace Solari.Hyperion.Abstractions
{
    public static class HyperionLogger
    {
        public static class HostedServiceLogger
        {
            private const string Prefix = "Solari Hyperion (HostedService)";

            public static void LogRegisteringService(string id) { Log.Debug($"{Prefix}Registering application into consul with id {id}"); }

            public static void LogDeregistering(string id) { Log.Debug($"{Prefix}Deregistering application with id {id} from consul"); }

            public static void LogDeregisterError(string id, string message) { Log.Error($"{Prefix}Filed to deregister service with id {id}. {message}"); }
        }
    }
}